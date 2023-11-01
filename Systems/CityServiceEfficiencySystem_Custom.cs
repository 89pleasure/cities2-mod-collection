using Game.City;
using Game.Common;
using Game.Economy;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.Buildings;
using Game;
using System.Runtime.CompilerServices;
using Colossal.Collections;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

namespace Cities2ModCollection.Systems
{
    [CompilerGenerated]
    public class CityServiceEfficiencySystem_Custom : GameSystemBase
    {
        private EntityQuery m_UpdatedBudgetQuery;
        private EntityQuery m_BuildingQuery;
        private EntityQuery m_ChangedBuildingQuery;
        private TypeHandle __TypeHandle;
        private EntityQuery __efficiencyParameterDataQuery;
        private EntityQuery __budgetDataQuery;

        [Preserve]
        protected override void OnCreate()
        {
            base.OnCreate();
            m_UpdatedBudgetQuery = GetEntityQuery(ComponentType.ReadOnly<ServiceBudgetData>(), ComponentType.ReadOnly<Updated>());
            m_ChangedBuildingQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[3]
                {
                    ComponentType.ReadOnly<CityServiceUpkeep>(),
                    ComponentType.ReadOnly<PrefabRef>(),
                    ComponentType.ReadWrite<Efficiency>()
                },
                Any = new ComponentType[2]
                {
                    ComponentType.ReadOnly<Created>(),
                    ComponentType.ReadOnly<Updated>()
                },
                None = new ComponentType[2]
                {
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });
            m_BuildingQuery = GetEntityQuery(ComponentType.ReadOnly<CityServiceUpkeep>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadWrite<Efficiency>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
            RequireAnyForUpdate(m_UpdatedBudgetQuery, m_ChangedBuildingQuery);
            RequireForUpdate<ServiceBudgetData>();
            RequireForUpdate<BuildingEfficiencyParameterData>();
        }

        [Preserve]
        protected override void OnUpdate()
        {
            BuildingEfficiencyParameterData singleton = __efficiencyParameterDataQuery.GetSingleton<BuildingEfficiencyParameterData>();
            __TypeHandle.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref CheckedStateRef);
            BuildingStateEfficiencyJob buildingStateEfficiencyJob = default;
            buildingStateEfficiencyJob.m_PrefabRefType = __TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
            buildingStateEfficiencyJob.m_InstalledUpgradeType = __TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
            buildingStateEfficiencyJob.m_EfficiencyType = __TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle;
            buildingStateEfficiencyJob.m_Prefabs = __TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
            buildingStateEfficiencyJob.m_ServiceObjectDatas = __TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup;
            buildingStateEfficiencyJob.m_ServiceUpkeepDatas = __TypeHandle.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup;
            buildingStateEfficiencyJob.m_ServiceBudgets = __budgetDataQuery.GetSingletonBuffer<ServiceBudgetData>(isReadOnly: true);
            buildingStateEfficiencyJob.m_ServiceBudgetEfficiencyFactor = singleton.m_ServiceBudgetEfficiencyFactor;
            BuildingStateEfficiencyJob jobData = buildingStateEfficiencyJob;
            EntityQuery query = ((!m_UpdatedBudgetQuery.IsEmptyIgnoreFilter) ? m_BuildingQuery : m_ChangedBuildingQuery);
            Dependency = JobChunkExtensions.ScheduleParallel(jobData, query, Dependency);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void __AssignQueries(ref SystemState state)
        {
            __efficiencyParameterDataQuery = state.GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[1] { ComponentType.ReadOnly<BuildingEfficiencyParameterData>() },
                Any = new ComponentType[0],
                None = new ComponentType[0],
                Disabled = new ComponentType[0],
                Absent = new ComponentType[0],
                Options = EntityQueryOptions.IncludeSystems
            });
            __budgetDataQuery = state.GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[1] { ComponentType.ReadWrite<ServiceBudgetData>() },
                Any = new ComponentType[0],
                None = new ComponentType[0],
                Disabled = new ComponentType[0],
                Absent = new ComponentType[0],
                Options = EntityQueryOptions.IncludeSystems
            });
        }

        protected override void OnCreateForCompiler()
        {
            base.OnCreateForCompiler();
            __AssignQueries(ref CheckedStateRef);
            __TypeHandle.__AssignHandles(ref CheckedStateRef);
        }

        [Preserve]
        public CityServiceEfficiencySystem_Custom()
        {
        }

        [BurstCompile]
        private struct BuildingStateEfficiencyJob : IJobChunk
        {
            [ReadOnly]
            public ComponentTypeHandle<PrefabRef> m_PrefabRefType;

            [ReadOnly]
            public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;

            public BufferTypeHandle<Efficiency> m_EfficiencyType;

            [ReadOnly]
            public ComponentLookup<PrefabRef> m_Prefabs;

            [ReadOnly]
            public ComponentLookup<PrefabData> m_PrefabData;

            [ReadOnly]
            public ComponentLookup<ServiceObjectData> m_ServiceObjectDatas;

            [ReadOnly]
            public BufferLookup<ServiceUpkeepData> m_ServiceUpkeepDatas;

            [ReadOnly]
            public DynamicBuffer<ServiceBudgetData> m_ServiceBudgets;

            public AnimationCurve1 m_ServiceBudgetEfficiencyFactor;

            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                NativeArray<PrefabRef> nativeArray = chunk.GetNativeArray(ref m_PrefabRefType);
                BufferAccessor<InstalledUpgrade> bufferAccessor = chunk.GetBufferAccessor(ref m_InstalledUpgradeType);
                BufferAccessor<Efficiency> bufferAccessor2 = chunk.GetBufferAccessor(ref m_EfficiencyType);
                for (int i = 0; i < chunk.Count; i++)
                {
                    if (m_ServiceObjectDatas.TryGetComponent(nativeArray[i], out ServiceObjectData componentData))
                    {
                        float efficiency;
                        if (HasMoneyUpkeep(nativeArray[i]) || (bufferAccessor.Length != 0 && HasMoneyUpkeep(bufferAccessor[i])))
                        {
                            int serviceBudget = GetServiceBudget(componentData.m_Service);
                            if (serviceBudget > 50)
                            {
                                efficiency = m_ServiceBudgetEfficiencyFactor.Evaluate(serviceBudget / 100f);
                            }
                            else
                            {
                                efficiency = 1f;
                            }
                        }
                        else
                        {
                            efficiency = 1f;
                        }
                        
                        BuildingUtils.SetEfficiencyFactor(bufferAccessor2[i], EfficiencyFactor.ServiceBudget, efficiency);
                    }
                }
            }

            private bool HasMoneyUpkeep(Entity prefab)
            {
                if (m_ServiceUpkeepDatas.TryGetBuffer(prefab, out var bufferData))
                {
                    foreach (ServiceUpkeepData item in bufferData)
                    {
                        if (item.m_Upkeep.m_Resource == Resource.Money)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            private bool HasMoneyUpkeep(DynamicBuffer<InstalledUpgrade> installedUpgrades)
            {
                foreach (InstalledUpgrade item in installedUpgrades)
                {
                    if (m_Prefabs.TryGetComponent(item, out var componentData) && HasMoneyUpkeep(componentData))
                    {
                        return true;
                    }
                }

                return false;
            }

            private int GetServiceBudget(Entity service)
            {
                foreach (ServiceBudgetData serviceBudget in m_ServiceBudgets)
                {
                    if (serviceBudget.m_Service == service)
                    {
                        return serviceBudget.m_Budget;
                    }
                }

                return 100;
            }

            void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
            }
        }

        private struct TypeHandle
        {
            [ReadOnly]
            public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;

            [ReadOnly]
            public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
            public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RW_BufferTypeHandle;

            [ReadOnly]
            public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;

            [ReadOnly]
            public ComponentLookup<ServiceObjectData> __Game_Prefabs_ServiceObjectData_RO_ComponentLookup;

            [ReadOnly]
            public BufferLookup<ServiceUpkeepData> __Game_Prefabs_ServiceUpkeepData_RO_BufferLookup;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void __AssignHandles(ref SystemState state)
            {
                __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(isReadOnly: true);
                __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(isReadOnly: true);
                __Game_Buildings_Efficiency_RW_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>();
                __Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(isReadOnly: true);
                __Game_Prefabs_ServiceObjectData_RO_ComponentLookup = state.GetComponentLookup<ServiceObjectData>(isReadOnly: true);
                __Game_Prefabs_ServiceUpkeepData_RO_BufferLookup = state.GetBufferLookup<ServiceUpkeepData>(isReadOnly: true);
            }
        }
    }
}
