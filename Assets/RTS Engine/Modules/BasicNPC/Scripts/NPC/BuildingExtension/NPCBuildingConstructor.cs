﻿using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using RTSEngine.Determinism;
using RTSEngine.Entities;
using RTSEngine.EntityComponent;
using RTSEngine.Event;
using RTSEngine.NPC.UnitExtension;

namespace RTSEngine.NPC.BuildingExtension
{
    public class NPCBuildingConstructor : NPCComponentBase, INPCBuildingConstructor
    {
        #region Attributes 
        [SerializeField, EnforceType(typeof(IUnit), prefabOnly: true), Tooltip("List of units with the IBuilder component that the NPC faction can use to construct its buildings.")]
        private GameObject[] builders = new GameObject[0];
        private NPCActiveRegulatorMonitor builderMonitor;

        /// <summary>
        /// Key: key of the IBuilding prefab that can be placed by the above builders.
        /// Value: keys of the builder units that can place that building.
        /// </summary>
        public Dictionary<string, List<string>> buildingToBuilderCodes;
        public IReadOnlyList<string> GetBuildingBuilderCodes(string buildingCode)
        {
            buildingToBuilderCodes.TryGetValue(buildingCode, out List<string> builderCodes);
            return builderCodes;
        }

        // Holds a list of the NPC faction buildings that need construction
        private List<IBuilding> buildingsToConstruct = new List<IBuilding>();

        [SerializeField, Tooltip("How often does this component check whether there are buildings for the NPC faction to construct/repare?")]
        private FloatRange constructionTimerRange = new FloatRange(4.0f, 7.0f);
        private TimeModifiedTimer constructionTimer;

        [SerializeField, Tooltip("How many construction timer ticks are required for the NPC faction to enforce the builders amount by switching them from other tasks such as resource collection?")]
        private int enforceBuildersTimerTicks = 5;
        private int constructionTimerTicks;

        [SerializeField, Tooltip("Ratio of the amount of builders to send for construction to the maximum allowed amount of builders of a building.")]
        private FloatRange targetBuildersRatio = new FloatRange(0.5f, 0.8f);

        [SerializeField, Tooltip("When enabled, external components can request to construct buildings.")]
        private bool constructOnDemand = true;

        // NPC Components
        protected INPCUnitCreator npcUnitCreator { private set; get; }
        #endregion

        #region Initializing/Terminating
        protected override void OnPreInit()
        {
            this.npcUnitCreator = npcMgr.GetNPCComponent<INPCUnitCreator>();

            constructionTimer = new TimeModifiedTimer(constructionTimerRange);
        }
        protected override void OnPostInit()
        {
            ActivateBuilderRegulator();

            globalEvent.BuildingPlacedGlobal += HandleBuildingPlacedOrBuiltGlobal;
            globalEvent.BuildingBuiltGlobal += HandleBuildingPlacedOrBuiltGlobal;

            globalEvent.BuildingHealthUpdatedGlobal += HandleBuildingHealthUpdatedGlobal;

            for (int i = 0; i < factionMgr.Buildings.Count; i++)
            {
                OnBuildingConstructionStateUpdate(factionMgr.Buildings[i]);
            }
        }

        protected override void OnDestroyed()
        {
            globalEvent.BuildingPlacedGlobal -= HandleBuildingPlacedOrBuiltGlobal;
            globalEvent.BuildingBuiltGlobal -= HandleBuildingPlacedOrBuiltGlobal;

            globalEvent.BuildingHealthUpdatedGlobal -= HandleBuildingHealthUpdatedGlobal;

            builderMonitor.Disable();
        }

        private void ActivateBuilderRegulator()
        {
            builderMonitor = new NPCActiveRegulatorMonitor(gameMgr, factionMgr);

            buildingToBuilderCodes = new Dictionary<string, List<string>>();

            for (int i = 0; i < builders.Length; i++)
            {
                IUnit builder = builders[i].IsValid() ? builders[i].GetComponent<IUnit>() : null;
                if (!logger.RequireValid(builder,
                    $"[{GetType().Name} - {factionMgr.FactionID}] 'Builders' field has some unassigned elements."))
                    return;

                IBuilder builderComponent = builder.gameObject.GetComponentInChildren<IBuilder>();
                if (!logger.RequireValid(builderComponent,
                    $"[{GetType().Name} - {factionMgr.FactionID}] 'Builders' field has some assigned units with no component that implements '{typeof(IBuilder).Name}' interface attached to them."))
                    return;

                NPCUnitRegulator nextRegulator;
                if ((nextRegulator = npcUnitCreator.ActivateUnitRegulator(builder)).IsValid())
                {
                    builderMonitor.AddCode(nextRegulator.Prefab.Code);

                    // For each valid builder that gets added, get their available creation tasks.
                    List<BuildingCreationTask> availableCreationTasks = new List<BuildingCreationTask>(builderComponent.CreationTasks);
                    availableCreationTasks.AddRange(builderComponent.UpgradeTargetCreationTasks);
                    for (int j = 0; j < availableCreationTasks.Count; j++)
                    {
                        string buildingCode = availableCreationTasks[j].Prefab.Code;

                        if (!buildingToBuilderCodes.ContainsKey(buildingCode))
                            buildingToBuilderCodes.Add(buildingCode, new List<string>());

                        if (!buildingToBuilderCodes[buildingCode].Contains(builder.Code))
                        {
                            buildingToBuilderCodes[buildingCode].Add(builder.Code);
                        }
                    }
                }
            }

            if (builderMonitor.Count == 0)
                logger.LogWarning($"[{GetType().Name} - {factionMgr.FactionID}] No builder regulators have been assigned!");
        }
        #endregion

        #region Handling Events: Building Placed/Built, Building Health Update
        private void HandleBuildingPlacedOrBuiltGlobal(IBuilding building, EventArgs args)
            => OnBuildingConstructionStateUpdate(building);

        private void HandleBuildingHealthUpdatedGlobal(IBuilding building, HealthUpdateArgs args)
        {
            if (!factionMgr.IsSameFaction(building)
                || args.Value >= 0.0f)
                return;

            OnBuildingConstructionStateUpdate(building);
        }
        #endregion

        #region Building Construction
        private void OnBuildingConstructionStateUpdate(IBuilding building)
        {
            if (building.IsPlacementInstance
                || !factionMgr.IsSameFaction(building))
                return;

            if (building.Health.CurrHealth < building.Health.MaxHealth)
                OnBuildingEnterConstruction(building);
            else
                OnBuildingExitConstruction(building);
        }

        private void OnBuildingEnterConstruction(IBuilding building)
        {
            if (buildingsToConstruct.Contains(building))
                return;

            buildingsToConstruct.Add(building);

            building.WorkerMgr.WorkerRemoved += HandleConstructionBuildingWorkerRemoved;

            IsActive = true;
        }

        private void OnBuildingExitConstruction(IBuilding building)
        {
            buildingsToConstruct.Remove(building);

            building.WorkerMgr.WorkerRemoved -= HandleConstructionBuildingWorkerRemoved;
        }

        // Whenever an under-construction building has one worker less then check if it still requires construction or not.
        private void HandleConstructionBuildingWorkerRemoved(IEntity sender, EntityEventArgs<IUnit> args)
        {
            OnBuildingConstructionStateUpdate(sender as IBuilding);
        }

        public bool IsBuildingUnderConstruction(IBuilding building) => buildingsToConstruct.Contains(building);

        protected override void OnActiveUpdate()
        {
            //checking buildings timer:
            if (constructionTimer.ModifiedDecrease())
            {
                constructionTimer.Reload(constructionTimerRange);

                constructionTimerTicks++;

                bool canEnforceBuilders = constructionTimerTicks == enforceBuildersTimerTicks;
                if (canEnforceBuilders)
                    constructionTimerTicks = 0;

                // Only keep this component active if there are buildings that need construction
                IsActive = buildingsToConstruct.Count > 0;

                for (int i = 0; i < buildingsToConstruct.Count; i++)
                {
                    int targetBuildersAmount = GetTargetBuildersAmount(buildingsToConstruct[i]);

                    if (targetBuildersAmount > buildingsToConstruct[i].WorkerMgr.Amount)
                        OnBuildingConstructionRequestInternal(
                            buildingsToConstruct[i],
                            targetBuildersAmount,
                            out _,
                            forceSwitch: canEnforceBuilders);

                }
            }
        }


        public int GetTargetBuildersAmount(IBuilding building)
        {
            return Mathf.Max((int)(building.WorkerMgr.MaxAmount * targetBuildersRatio.RandomValue), 1);
        }

        public void OnBuildingConstructionRequest(IBuilding building, int targetBuildersAmount, out int assignedBuilders, bool forceSwitch = false)
        {
            assignedBuilders = 0;

            if (!building.IsValid())
                return;

            if (!constructOnDemand)
            {
                LogEvent($"{building.Code}: Unable to construct building on demand!");
                return;
            }

            OnBuildingConstructionRequestInternal(building, targetBuildersAmount, out assignedBuilders, forceSwitch);
        }

        private void OnBuildingConstructionRequestInternal(IBuilding building, int targetBuildersAmount, out int assignedBuilders, bool forceSwitch = false)
        {
            assignedBuilders = 0;

            if (!building.IsValid())
                return;

            if (building.Health.IsDead
                || building.Health.CurrHealth >= building.Health.MaxHealth)
            {
                LogEvent($"{building.Code}: Unable to construct building! Building Dead? {building.Health.IsDead} - Max Health Reached? {building.Health.CurrHealth >= building.Health.MaxHealth}");
                return;
            }

            int requiredBuilders = targetBuildersAmount - building.WorkerMgr.Amount;

            var nextBuilderRegulator = npcUnitCreator
                .GetActiveUnitRegulator(builderMonitor.RandomCode);

            if (!nextBuilderRegulator.IsValid())
            {
                LogEvent($"{building.Code}: Unable to find valid unit regulator for builder unit!");
                return;
            }

            for (int i = 0; i < nextBuilderRegulator.InstancesIdleFirst.Count; i++)
            {
                IUnit nextBuilder = nextBuilderRegulator.InstancesIdleFirst[i];

                bool canStillEnforceSwitch = forceSwitch
                    && !nextBuilder.BuilderComponent.HasTarget;

                if (nextBuilder.IsValid()
                    && (nextBuilder.IsIdle || canStillEnforceSwitch)
                    && nextBuilder.BuilderComponent.Target.instance != building)
                {
                    nextBuilder.BuilderComponent.SetTarget(building.ToTargetData(), playerCommand: false);

                    requiredBuilders--;
                    assignedBuilders++;

                    if (requiredBuilders <= 0)
                        break;
                }
            }

            LogEvent($"{building.Code}: Sent {assignedBuilders}/{assignedBuilders+requiredBuilders} builders to construct building!");
        }
        #endregion

        #region Logging
        [SerializeField, ReadOnly]
        private GameObject[] targetBuildings = new GameObject[0];

        protected override void UpdateActiveLogs()
        {
            targetBuildings = buildingsToConstruct
                .Where(building => building.IsValid())
                .Select(building => building.gameObject)
                .ToArray();
        }
        #endregion
    }
}
