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

== aintro ==
    =tree
    it's just a tree
    -> END
== devdoor ==
    Sadly this is the current end of this story
    Quite pitful isn't it...
    What's is inside the pyramid
    Will Wilbursoot form a cult that last more than a one episode
    And will Technoblade get his cookies
    I hope you enjoyed what little I had to show you and that you will one day come back to play a more finished game
    -> END
== thebell ==
    = thefirstbell
        ~playSound("sellouttimer")
        ~displayPortrait("Technoblade", "default")
        ~setTextSound("technoblade")
        #unskipable
        #slow
        . . .
        ~setTextSound("bell")
        #technoblade
        subscribe to Technoblade!!!!!
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
        
    

    
























