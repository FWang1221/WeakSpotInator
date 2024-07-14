using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using SoulsFormats;
using WeakSpotInator;

namespace Limbs
{
    internal class Program
    {
        public static void readHKXPWVAndOutputFiles(String chrbndPath)
        {

            BND4 chrbnd = BND4.Read(chrbndPath);
            var limbFile = new BinderFile();
            try
            {
                limbFile = chrbnd.Files.Where(
                    x => x.Name.Contains("hkxpwv")
                )
                .First();
            }
            catch
            {
                return; //returns if no hkxpwv
            }

            HKXPWV limbs = new HKXPWV();

            limbs.Game = HKXPWV.GameType.DS3;

            limbs = HKXPWV.Read(
                limbFile.Bytes
                );

            

            // Define the regex pattern to extract the desired characters
            string pattern = @"([^\\]+)\.chrbnd\.dcx$";

            // Match the pattern against the file path
            Match match = Regex.Match(chrbndPath, pattern);
            string character = "";
            if (match.Success)
            {
                // Extract the matched group (characters before ".chrbnd.dcx")
                character = match.Groups[1].Value;

                Console.WriteLine("Character: " + character);
            }
            else
            {
                Console.WriteLine("File path doesn't match the expected pattern (cXXXX.chrbnd.dcx)");
            }

            Weakspot weakspot = new Weakspot(limbs, chrbndPath);
            HKXPWV.RagdollBoneEntry nutsToHeadshot = new HKXPWV.RagdollBoneEntry();
            nutsToHeadshot.DisableHit = false;
            nutsToHeadshot.DamageAnimID = -1;
            nutsToHeadshot.DisableCollision = false;
            nutsToHeadshot.NPCPartDamageGroup = 6;
            nutsToHeadshot.NPCPartGroupIndex = 28;
            nutsToHeadshot.RagdollParamID = -1;
            nutsToHeadshot.UnknownBB = -1;

            if (weakspot.editMapping(
                "R_Thigh",
                "Ragdoll_R_Thigh001",
                nutsToHeadshot
                )) {
                Console.WriteLine(character + " has nuts");
            }
            if (weakspot.editMapping(
                "R_Calf",
                "Ragdoll_R_Calf001",
                nutsToHeadshot
                ))
            {
                Console.WriteLine(character + " has nuts");
            }
            if (weakspot.editMapping(
                "R_Foot",
                "Ragdoll_R_Foot001",
                nutsToHeadshot
                ))
            {
                Console.WriteLine(character + " has nuts");
            }

            weakspot.writeToFiles();
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string filePathStd = @".\outputs\" + character + ".json";

            // Write the serialized JSON to a file
            File.WriteAllText(
                filePathStd,
                JsonSerializer.Serialize(limbs, options)
                );

            string filePathWkPt = @".\outputsWeakPoints\" + character + ".json";

            // Write the serialized JSON to a file
            File.WriteAllText(
                filePathWkPt,
                JsonSerializer.Serialize(weakspot.boneSpots, options)
                );


        }
        static void Main(string[] args)
        {
            try
            {
                string[] files = Directory.GetFiles(Environment.CurrentDirectory, "*.chrbnd.dcx");

                if (files.Length > 0)
                {
                    Console.WriteLine("Files with .chrbnd.dcx extension:");

                    foreach (string file in files)
                    {
                        try
                        {
                            readHKXPWVAndOutputFiles(file);
                        } catch (Exception e)
                        {
                            Console.WriteLine("An error occurred: " + e.Message + "\nStack Trace: " + e.StackTrace);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No files found with .chrbnd.dcx extension.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
            }

            Console.ReadLine();

        }
    }
}
