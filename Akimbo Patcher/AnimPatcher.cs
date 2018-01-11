using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows;
using SELib;
using SELib.Utilities;

namespace Akimbo_Patcher
{
    class AnimPatcher
    {
        /// <summary>
        /// List of Arm Joints to replace based of selected Arm.
        /// </summary>
        public static Dictionary<string, string[]> ArmReplacements = new Dictionary<string, string[]>()
        {
            // Joints to replace if we're processing Left animation
            { "Left", new string[]{
                "j_shoulder_ri",
                "j_elbow_ri",
                "j_wrist_ri",
                "j_index_ri_0",
                "j_index_ri_1",
                "j_index_ri_2",
                "j_index_ri_3",
                "j_mid_ri_0",
                "j_mid_ri_1",
                "j_mid_ri_2",
                "j_mid_ri_3",
                "j_pinkypalm_ri",
                "j_pinky_ri_0",
                "j_pinky_ri_1",
                "j_pinky_ri_2",
                "j_pinky_ri_3",
                "j_ringpalm_ri",
                "j_ring_ri_0",
                "j_ring_ri_1",
                "j_ring_ri_2",
                "j_ring_ri_3",
                "j_thumb_ri_0",
                "j_thumb_ri_1",
                "j_thumb_ri_2",
                "j_thumb_ri_3",
                "tag_knife_attach2",
                "tag_weapon_right",
                "j_wristtwist_ri" } },
            // Joints to replace if we're processing right animation
            { "Right", new string[]{
                "j_shoulder_le",
                "j_elbow_le",
                "j_wrist_le",
                "j_index_le_0",
                "j_index_le_1",
                "j_index_le_2",
                "j_index_le_3",
                "j_mid_le_0",
                "j_mid_le_1",
                "j_mid_le_2",
                "j_mid_le_3",
                "j_pinkypalm_le",
                "j_pinky_le_0",
                "j_pinky_le_1",
                "j_pinky_le_2",
                "j_pinky_le_3",
                "j_ringpalm_le",
                "j_ring_le_0",
                "j_ring_le_1",
                "j_ring_le_2",
                "j_ring_le_3",
                "j_thumb_le_0",
                "j_thumb_le_1",
                "j_thumb_le_2",
                "j_thumb_le_3",
                "tag_knife_attach",
                "tag_weapon_left",
                "j_wristtwist_le", } },
        };

        public static string GetNewJointName(string jointName, string arm)
        {
            // Opposite of current arm.
            string opposite = arm == "Left" ? "Right" : "Left";
            // New Bone Name
            string bone = jointName;
            // Have a better version of this, but need to patch it up
            // for now just do mutliple ifs/else's.
            // We're at opposite arm, cure it.
            if (ArmReplacements[arm].Contains(jointName))
            {
                bone += "_dud_jnt";
            }
            // We're at current arm joint, leave it.
            else if (ArmReplacements[opposite].Contains(jointName))
            {
                bone = jointName;
            }
            // Rename j_gun if we're on left arm.
            else if (jointName == "j_gun" && arm == "Left")
            {
                bone = "tag_weapon_le";
            }
            // Rename tag_flash if we're on left arm.
            else if (jointName == "tag_flash" && arm == "Left")
            {
                bone = "tag_flash_le";
            }
            // Rename tag_brass if we're on left arm.
            else if (jointName == "tag_brass" && arm == "Left")
            {
                bone = "tag_brass_le";
            }
            // Everything else add 1.
            else if (arm == "Left")
            {
                bone += "1";
            }
            // Return
            return bone;
        }

        public static void ProcessAkimboSEAnims(string[] animFiles, string outputDirectory, string arm)
        {
            var stopWatch = System.Diagnostics.Stopwatch.StartNew();
            // Process Animation Files
            foreach(string animFile in animFiles)
            {
                // Resulting Animation
                SEAnim outputAnim   = new SEAnim();
                // Read SEAnim
                SEAnim inputAnim    = SEAnim.Read(animFile);
                // Copy Bone Modifiers
                foreach (var BoneModifier in inputAnim.AnimationBoneModifiers)
                    if (!outputAnim.AnimationBoneModifiers.ContainsKey(BoneModifier.Key))
                        outputAnim.AnimationBoneModifiers.Add(GetNewJointName(BoneModifier.Key, arm), BoneModifier.Value);
                // Copy Bone Data
                foreach(string bone in inputAnim.Bones)
                {
                    // Get Patched joint name
                    string newName = GetNewJointName(bone, arm);
                    // Copy Translation Data
                    if (inputAnim.AnimationPositionKeys.ContainsKey(bone))
                        foreach (SEAnimFrame frame in inputAnim.AnimationPositionKeys[bone])
                            outputAnim.AddTranslationKey(
                                newName,
                                frame.Frame,
                                ((Vector3)frame.Data).X,
                                ((Vector3)frame.Data).Y,
                                ((Vector3)frame.Data).Z);
                    // Add Rotation Keys
                    if (inputAnim.AnimationRotationKeys.ContainsKey(bone))
                        foreach (SEAnimFrame frame in inputAnim.AnimationRotationKeys[bone])
                            outputAnim.AddRotationKey(
                                newName,
                                frame.Frame,
                                ((Quaternion)frame.Data).X,
                                ((Quaternion)frame.Data).Y,
                                ((Quaternion)frame.Data).Z,
                                ((Quaternion)frame.Data).W);
                    // Add Scale Keys
                    if (inputAnim.AnimationScaleKeys.ContainsKey(bone))
                        foreach (SEAnimFrame frame in inputAnim.AnimationScaleKeys[bone])
                            outputAnim.AddScaleKey(
                                newName,
                                frame.Frame,
                                ((Vector3)frame.Data).X,
                                ((Vector3)frame.Data).Y,
                                ((Vector3)frame.Data).Z);
                }
                // Copy Notetracks
                foreach (KeyValuePair<string, List<SEAnimFrame>> Notetrack in inputAnim.AnimationNotetracks)
                    foreach (SEAnimFrame NoteFrame in Notetrack.Value)
                        outputAnim.AddNoteTrack(Notetrack.Key, NoteFrame.Frame);
                // Write new Anim
                outputAnim.Write(Path.Combine(outputDirectory, Path.GetFileName(animFile)));
            }
            stopWatch.Stop();
            string msg = string.Format("{0} Animation files patched in {1} seconds.",
                animFiles.Length, stopWatch.ElapsedMilliseconds / 1000.000);
            MessageBox.Show(msg, "Complete", System.Windows.MessageBoxButton.OK);
        }
    }
}
