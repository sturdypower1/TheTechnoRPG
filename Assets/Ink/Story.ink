-> DONE
INCLUDE BeforeThePyramid.ink

EXTERNAL levelUp()

VAR attackBonus = 0
VAR defenceBonus = 0
VAR pointsBonus = 0
VAR healthBonus = 0
VAR skillName = ""
VAR characterName = ""
VAR itemName = ""
VAR totalEXP = 0
VAR totalGold = 0

EXTERNAL updateEXPGold()
EXTERNAL playSound(soundName)
EXTERNAL GetNextBattleItem()
EXTERNAL displayPortrait(characterName, feeling)
EXTERNAL setTextSound(soundName)
EXTERNAL playSong(songName)
EXTERNAL healPlayers()

== defaultbell
hearing the sell out time go off, Technoblade rings the bell
~playSound("Heal")
~healPlayers()
#instant
Through the power of subscribers, The party was healed
->DONE
== aintro ==
    =tree
    it's just a tree
    -> DONE
== devdoor ==
    Sadly this is the current end of this story
    I have alot of the work for the next level already done, I just need to get a few things done
    Hopefully I get it done soon so you can enjoy a much more finished game
    -> DONE
=== function updateLevelInfo(name, SkillName, AttackBonus, DefenceBonus, PointBonus, HealthBonus)
     ~ characterName = name
     ~ healthBonus = HealthBonus
     ~ skillName = SkillName
     ~ attackBonus = AttackBonus
     ~ defenceBonus= DefenceBonus
     ~ pointsBonus = PointBonus
=== function updateItemInfo(ItemName)
    ~ itemName = ItemName
=== function updateEXPandGold(exp, gold)
    ~ totalEXP = exp
    ~ totalGold = gold
== victory ==
~updateEXPGold()
~displayPortrait("empty", "default")
You Won
Technoblade gained {totalEXP} EXP and {totalGold} gold
-> items
    = items
        ~GetNextBattleItem()
        {itemName != "": Obtained {itemName}| -> levelupdetails}
        -> items
    = levelupdetails
        ~levelUp()
        {characterName == "": ->DONE | {characterName} leveled up!}
        {characterName} gained {attackBonus} attack, {defenceBonus} defence, {pointsBonus} blood, and {healthBonus} hp
        {skillName != "": {characterName} unlocked {skillName}| ->levelupdetails}
        ->levelupdetails
        
    

    
























