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
== tutorial ==
    =textbox1   
        works how you'd think a text box would work. Just remember that you can skip to the end of dialogue by clicking with your mouse.
        Also if your afraid of losing progress you can save at any bell!
        ->DONE
    =techno1
        This button is for a basic attack! Dealing a small amount of damage and inflicting the bleeding status effect
        -> DONE
    =techno2
        This button is for your skills! Use blood to perform special attacks on your opponents
        ->DONE
    =techno3
        This indicates that an enemy is affected by the bleeding status effect. Every so often they will take bleeding damage accorrding to how many times they've been afflitcted with the bleeding status effect. All damage done from bleeding is put into Technoblade blood meter, allowing him to perform his skills.
        ->DONE
    =techno4
        This button is for items! Using a item will typically end up healing Technoblade.
        ->DONE
    =techno5
        This button is for running! However I haven't yet implemented it... But once it is it'll probably be used to run from insignificant battles
        ->DONE
    =techno6
        Technoblade doesn't die once his hp drops to 0. Instead the blood meter will used for his health instead. He will then only die once his blood reaches 0. 
        ->DONE
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
        
    

    
























