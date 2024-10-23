-- utilities is a struct that gives out a helpful replacement for print (which doesn't work)
utilities:print(characterEdited) -- Which character to edit, like c4750 for Godrick. You can use this to check for which character you should be editing.

-- hkxpwvEdited.ragdollBoneEntryMaker(bool DisableHit, short DamageAnimID, bool DisableCollision, byte NPCPartDamageGroup, byte NPCPartGroupIndex, int RagdollParamID, sbyte UnknownBB) returns a HKXPWV.RagdollBoneEntry, which is a weak point that you can assign to the hkxpwvEdited (well for the lua api it's all ints and bools but it gets casted so it's fine)
--hkxpwvEdited is the HKXPWV class you'll have access to. It's basically your HKXPWV for editing

-- ragdollBoneEntry that effectively functions as a headshot
local thing = hkxpwvEdited:ragdollBoneEntryMaker(false, -1, false, 9, 31, -1, -1)
print(thing.NPCPartGroupIndex)

-- hkxpwvEdited:editMapping(string boneAName, string boneBName, SoulsFormats.HKXPWV.RagdollBoneEntry ragBoneEntry) returns either true or false, depending on whether the edit worked or not. boneAName is the name of bone A to apply your ragBoneEntry (which comes from the above function) to

-- applies the headshot modifier to the penis. you can obtain the names of bone A and bone B from the outputs. checks if it was successful
local success = hkxpwvEdited:editMapping("Pelvis", "Ragdoll_Pelvis001", thing)

-- hkxpwvEdited:writeToFiles() should be called at the end of the script, to apply your changes

if success then
    utilities:print(characterEdited .. " was penis-inated!")
    -- writes to the file
    hkxpwvEdited:writeToFiles()
else
    utilities:print("Something fucked up...")
end

-- notes:

-- use colon (heh heh colon) operator instead of dot operator for calling exposed (utilities, hkxpwvEdited) if you don't wanna have a bad time

-- drag a folder containing your chrbnds to get JSON outputs and stuff from your chrbnds telling you weak points and shit

-- drag a folder containing your chrbnds and a lua file similar to this one to basically run the lua file against each of the files
