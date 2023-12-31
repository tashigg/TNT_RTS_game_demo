﻿using UnityEngine;

using RTSEngine.Entities;
using RTSEngine.Game;
using RTSEngine.Search;
using RTSEngine.Determinism;

namespace RTSEngine.EntityComponent
{
    /// <summary>
    /// Allows to define a search process (for an Entity instance).
    /// </summary>
    [System.Serializable]
    public class TargetEntityFinder<T> where T : IEntity
    {
        [SerializeField, Tooltip("Enable finding targets and set search period time.")]
        private GlobalTimeModifiedTimer reload = new GlobalTimeModifiedTimer(enabled: true);

        public IEntityTargetComponent Source { get; private set; }

        private bool enabled = false;
        /// <summary>
        /// Set or get whether searching for a target entity is active or not.
        /// </summary>
        public bool Enabled { 
            set 
            {
                enabled = value;
                reload.IsActive = enabled;
            }
            get => enabled;
        }

        // Where the search will be conducted from.
        private Transform center;
        public Transform Center
        {
            set
            {
                if (value.IsValid())
                    center = value;
            }
            get => center.IsValid() ? center : Source.Entity.transform;
        }

        public bool PlayerCommand { set; get; }

        private float range;
        /// <summary>
        /// 
        /// </summary>
        public float Range
        {
            set
            {
                if (value > 0.0f)
                    range = value;
            }
            get => range;
        }

        public bool IdleOnly { set; get; }

        public float ReloadTime { private set; get; }
        public TargetEntityFinderData Data => new TargetEntityFinderData
        {
            enabled = Enabled,
            idleOnly = IdleOnly,
            range = range,
            reloadTime = ReloadTime
        };

        protected IGridSearchHandler gridSearch { private set; get; }

        /// <summary>
        /// Initializes a TargetEntityFinder instance.
        /// </summary>
        /// <param name="source">Entity instance that the TargetEntityFinder is finding a target for.</param>
        public TargetEntityFinder (IGameManager gameMgr, IEntityTargetComponent source, Transform center, TargetEntityFinderData data)
        {
            this.gridSearch = gameMgr.GetService<IGridSearchHandler>(); 

            this.Source = source;
            this.Center = center;
            this.PlayerCommand = false;

            // We start a small random range for the reload and then set the proper reload after the first search
            reload.Init(gameMgr, SearchTarget, UnityEngine.Random.Range(0.1f, 0.25f));
            ReloadTime = data.reloadTime;
            Range = data.range;
            IdleOnly = data.idleOnly;

            Enabled = data.enabled && source.IsActive;
        }

        private void SearchTarget()
            => SearchTarget(Data);

        public void SearchTarget(TargetEntityFinderData nextSearchData)
        {
            if (RTSHelper.IsMasterInstance()
                && Source.Entity.CanLaunchTask
                && !Source.HasTarget
                && (!nextSearchData.idleOnly || Source.Entity.IsIdle)
                && Source.CanSearch
                && gridSearch.Search(Center.position, nextSearchData.range, Source.IsTargetValid, playerCommand: false, out T potentialTarget) == ErrorMessage.none)
            {
                Source.SetTarget(new TargetData<IEntity> { instance = potentialTarget, position = potentialTarget.transform.position }, playerCommand: PlayerCommand);
            }

            // Reload timer
            if (Enabled)
            {
                reload.SetDefaultValue(Data.reloadTime);
                reload.IsActive = true;
            }
        }
    }
}
