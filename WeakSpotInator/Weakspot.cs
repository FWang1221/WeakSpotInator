using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;
using HKLib.Serialization.hk2018.Binary;
using HKLib.Serialization.hk2018.Xml;
using HKLib.hk2018;
using System.Text.Json;
using System.Reflection;
using System.Numerics;
using System.Runtime.Intrinsics;

namespace WeakSpotInator
{
    internal class Weakspot
    {
        public HKXPWV HKXPWV { get; set; }
        public HKLib.hk2018.hkRootLevelContainer.NamedVariant skeletonMapper { get; set; }
        private List<HKLib.hk2018.hkaSkeletonMapperData.SimpleMapping> simpleMappings { get; set; }
        private HKLib.hk2018.hkaSkeleton skeletonA { get; set; }
        private HKLib.hk2018.hkaSkeleton skeletonB { get; set; }

        private BND4 bonesHKX { get; set; }
        private string chrbndPath { get; set; }
        private BinderFile BinderFile { get; set; }
        private HKLib.hk2018.IHavokObject havokObj { get; set; }

        public List<boneSpot> boneSpots { get; private set; } = new List<boneSpot>();
        public struct boneSpot {
            public HKXPWV.RagdollBoneEntry ragBones { get; set; }
            public hkaBoneMimic hkaBoneA { get; set; }
            public hkaBoneMimic hkaBoneB { get; set; }
            public hkQsTransformationMimic transform { get; set; }
        }
        // this is really fucking stupid but otherwise the havokObject shit is empty
        public struct hkaBoneMimic {
            public bool m_lockTranslation { get; set; }
            public string m_name { get; set; }
        }

        public struct hkQsTransformationMimic { 
            public Object m_rotation { get; set; }
            public Object m_scale { get; set; }
            public Object m_translation { get; set; }
        }
        public Weakspot(HKXPWV hkxpwv, string chrbndPath) { 
        
            this.HKXPWV = hkxpwv;
            this.havokObj = readHKXSkeletonTest(chrbndPath);
            HKLib.hk2018.hkRootLevelContainer havokObjContainer = (HKLib.hk2018.hkRootLevelContainer)havokObj;

            IEnumerable <HKLib.hk2018.hkRootLevelContainer.NamedVariant>
                hkaSkeleMappers = havokObjContainer
                    .m_namedVariants
                    .Where(
                        x => x
                            .m_name
                            .Equals("SkeletonMapper")
            );

            HKLib.hk2018.hkRootLevelContainer.NamedVariant hkaSkeleMapper1 = hkaSkeleMappers.First();

            this.skeletonMapper = hkaSkeleMapper1;

            setMappings();

        }

        private void setMappings() {

            HKLib.hk2018.hkaSkeletonMapper hkaSkeletonMapper = (HKLib.hk2018.hkaSkeletonMapper)this.skeletonMapper.m_variant;

            this.simpleMappings = hkaSkeletonMapper.m_mapping.m_simpleMappings;
            this.skeletonA = hkaSkeletonMapper.m_mapping.m_skeletonA;
            this.skeletonB = hkaSkeletonMapper.m_mapping.m_skeletonB;

            int mapCounter = 0;
            boneSpots.Clear();
            foreach (HKLib.hk2018.hkaSkeletonMapperData.SimpleMapping simpleMapping in this.simpleMappings) {
                boneSpot boneSpot = new boneSpot();
                boneSpot.ragBones = this.HKXPWV.RagdollBoneEntries[mapCounter];

                HKLib.hk2018.hkaBone boneA = this.skeletonA.m_bones[simpleMapping.m_boneA];
                hkaBoneMimic boneAMimic = new hkaBoneMimic();
                boneAMimic.m_lockTranslation = boneA.m_lockTranslation;
                boneAMimic.m_name = boneA.m_name;
                HKLib.hk2018.hkaBone boneB = this.skeletonB.m_bones[simpleMapping.m_boneB];
                hkaBoneMimic boneBMimic = new hkaBoneMimic();
                boneBMimic.m_lockTranslation = boneB.m_lockTranslation;
                boneBMimic.m_name = boneB.m_name;

                boneSpot.hkaBoneA = boneAMimic;
                boneSpot.hkaBoneB = boneBMimic;

                hkQsTransformationMimic transformMimic = new hkQsTransformationMimic();
                var serializedQuaternionRotation = new
                {
                    X = simpleMapping.m_aFromBTransform.m_rotation.X,
                    Y = simpleMapping.m_aFromBTransform.m_rotation.Y,
                    Z = simpleMapping.m_aFromBTransform.m_rotation.Z,
                    W = simpleMapping.m_aFromBTransform.m_rotation.W,
                    IsIdentity = simpleMapping.m_aFromBTransform.m_rotation.IsIdentity
                };
                var serializedVectorScale = new
                {
                    X = simpleMapping.m_aFromBTransform.m_scale.X,
                    Y = simpleMapping.m_aFromBTransform.m_scale.Y,
                    Z = simpleMapping.m_aFromBTransform.m_scale.Z,
                    W = simpleMapping.m_aFromBTransform.m_scale.W
                };
                var serializedVectorTranslation = new
                {
                    X = simpleMapping.m_aFromBTransform.m_translation.X,
                    Y = simpleMapping.m_aFromBTransform.m_translation.Y,
                    Z = simpleMapping.m_aFromBTransform.m_translation.Z,
                    W = simpleMapping.m_aFromBTransform.m_translation.W
                };
                transformMimic.m_rotation = serializedQuaternionRotation;
                transformMimic.m_scale = serializedVectorScale;
                transformMimic.m_translation = serializedVectorTranslation;

                boneSpot.transform = transformMimic;

                boneSpots.Add(boneSpot);
                mapCounter++;
            }
        }
        //method currently doesn't fully work due to me not being able to save the hkx file somehow
        public void addMapping(string boneAName, string boneBName, SoulsFormats.HKXPWV.RagdollBoneEntry ragBoneEntry, System.Numerics.Vector4 translation) {

            this.HKXPWV.RagdollBoneEntries.Add(ragBoneEntry);

            HKLib.hk2018.hkQsTransform qsTransformAuto = new hkQsTransform();
            qsTransformAuto.m_rotation = new System.Numerics.Quaternion(0, 0, 0, 1); 
            //nearly all are like this (I think the rotation and scale are off due to floating point) with the exception of translaton which is set proper.
            qsTransformAuto.m_scale = new System.Numerics.Vector4(1, 1, 1, 0);
            qsTransformAuto.m_translation = translation;

            HKLib.hk2018.hkaSkeletonMapperData.SimpleMapping simpleMapping = new hkaSkeletonMapperData.SimpleMapping();
            simpleMapping.m_aFromBTransform = qsTransformAuto;
            simpleMapping.m_boneA = (short)this.skeletonA
                .m_bones
                .IndexOf(
                    this.skeletonA.m_bones
                        .Where(
                            x => x.m_name.Equals(boneAName)
                        )
                        .First()
                        );

            simpleMapping.m_boneB = (short)this.skeletonB
                .m_bones
                .IndexOf(
                    this.skeletonB.m_bones
                        .Where(
                            x => x.m_name.Equals(boneBName)
                        )
                        .First()
                        );

            this.simpleMappings.Add(
                simpleMapping
                );

        }

        public bool editMapping(string boneAName, string boneBName, SoulsFormats.HKXPWV.RagdollBoneEntry ragBoneEntry) {

            try
            {

                // Find the indices of boneAName and boneBName in skeletonA and skeletonB
                int indexA = this.skeletonA.m_bones.FindIndex(x => x.m_name.Equals(boneAName));
                int indexB = this.skeletonB.m_bones.FindIndex(x => x.m_name.Equals(boneBName));

                // Find the corresponding SimpleMapping based on the indices
                HKLib.hk2018.hkaSkeletonMapperData.SimpleMapping correspondingMapping = this.simpleMappings.FirstOrDefault(mapping =>
                    mapping.m_boneA == indexA && mapping.m_boneB == indexB);

                if (correspondingMapping != null)
                {
                    // Retrieve the RagdollBoneEntry using the corresponding mapping
                    SoulsFormats.HKXPWV.RagdollBoneEntry ragBone = this.HKXPWV.RagdollBoneEntries[this.simpleMappings.IndexOf(correspondingMapping)];

                    ragBone.DisableCollision = ragBoneEntry.DisableCollision;
                    ragBone.UnknownBB = ragBoneEntry.UnknownBB;
                    ragBone.NPCPartGroupIndex = ragBoneEntry.NPCPartGroupIndex;
                    ragBone.DamageAnimID = ragBoneEntry.DamageAnimID;
                    ragBone.NPCPartDamageGroup = ragBoneEntry.NPCPartDamageGroup;
                    ragBone.RagdollParamID = ragBoneEntry.RagdollParamID;
                    ragBone.DisableHit = ragBoneEntry.DisableHit;
                    return true;
                }
                return false;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public HKXPWV.RagdollBoneEntry ragdollBoneEntryMaker(bool DisableHit, int DamageAnimID, bool DisableCollision, int NPCPartDamageGroup, int NPCPartGroupIndex, int RagdollParamID, int UnknownBB)
        {

            HKXPWV.RagdollBoneEntry ragBoneEntry = new HKXPWV.RagdollBoneEntry();
            ragBoneEntry.DisableHit = DisableHit;
            ragBoneEntry.DamageAnimID = (short)DamageAnimID;
            ragBoneEntry.DisableCollision = DisableCollision;
            ragBoneEntry.NPCPartDamageGroup = (byte)NPCPartDamageGroup;
            ragBoneEntry.NPCPartGroupIndex = (byte)NPCPartGroupIndex;
            ragBoneEntry.RagdollParamID = (int)RagdollParamID;
            ragBoneEntry.UnknownBB = (sbyte) UnknownBB;

            return ragBoneEntry;

        }

        public void writeToFiles() {
            var hkxpwvFile = this.bonesHKX.Files.Where(
                    x => x.Name.Contains("hkxpwv")
                )
                .First();
            hkxpwvFile.Bytes = this.HKXPWV.Write();
            this.bonesHKX.Write(this.chrbndPath);
        }

        public HKLib.hk2018.IHavokObject readHKXSkeletonTest(String chrbndPath)
        {
            BND4 chrbnd = BND4.Read(chrbndPath);
            this.bonesHKX = chrbnd;
            this.chrbndPath = chrbndPath;
            var limbFile = new BinderFile();
            try
            {
                limbFile = chrbnd.Files.Where(
                    x => x.Name.Contains(".hkx") && !x.Name.Contains("_c")
                )
                .First();
            }
            catch
            {
                return null; //returns if no hkxpwv
            }

            HavokBinarySerializer binarySerializer = new();
            using (MemoryStream stream = new MemoryStream(limbFile.Bytes))
            {
                return binarySerializer.Read(stream);
            }
        }
    }
}
