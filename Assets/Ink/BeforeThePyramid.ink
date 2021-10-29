

== parkedboats
    A set of boats that were meticulous for decoration 
    ->DONE
== partysign
    Welcome to Eret's birthday party!
    ->DONE
== thefirstbell
    ~playSound("sellouttimer")
    ~displayPortrait("Technoblade", "default")
    ~setTextSound("technoblade")
    #unskipable
    #slow
    . . .
    ~setTextSound("bell")
    #technoblade
    subscribe to Technoblade!!!!!
    #instant
    ~playSound("Heal")
    ~healPlayers()
    ~displayPortrait("empty", "default")
    Technoblade was somehow healed by this
    -> DONE
==sandbag1
    a big bag of fine sand. Its half eaten.
    -> DONE
== burningboat
    ~displayPortrait("Technoblade", "default")
    looks like somebody was moving the boat so fast they reenabled the old mechanics
    -> DONE
== nametag
    A nametag is burried into the ground. 
    The only letters visiable are "sky"
->DONE

== steveintro
    VAR dice_roll = 0
    ~dice_roll = RANDOM(1, 10)
    {
    - dice_roll == 1:
        ~displayPortrait("Steve", "fish")
        ~playSound("rawr")
        raaaaaaaauuugh
    - else: 
        ~displayPortrait("Steve", "default")
        ~playSound("rawr")
        raaaaaaaauuugh
    
    }
    ->DONE

== intro ==
        ~displayPortrait("Technoblade", "default")
        ~setTextSound("technoblade")
        Staaarting the streeeam
        technolate?
        Look, you can't blame me. I was inches from hitting the go live button, then my door flung open
        The super volcano underneath Yellow Stone Park is about to erupt
        You gotta believe me
        Luckily, the stuation is sorted
        No ancient volcano is going to stop Technoblade from streaming Minecraft
        Anyways, the note I got last time said to go here for some free fresh cookies
        It's a trap?
        Look it could be a trap and I'm ganged up on by 5 people and I lose one of my three lives.  
        Or I could get cookies
        I think the benefits vastly outweigh the potential downsides
        And even if this is all a rouse
        Steve is close by to provide back up
        ~displayPortrait("Steve", "default")
        ~playSound("rawr")
        raaaaaaaauuugh
        ~displayPortrait("Technoblade", "default")
        ~setTextSound("technoblade")
        There isn't anyway this could backfire
        Well Eret's pyramid is just around the corner, I just need to follow the path
        -> DONE
== tutorial ==
    =textbox1
    
    {TURNS_SINCE(-> thefirstbell) > 0:<color=red>ONE OF US! ONE OF US! ONE OF US! ONE OF US! ONE OF US!</color>|<color=red>works how you'd think a text box would work. Just remember that you can skip to the end of dialogue by clicking with your mouse.</color>} 
        ->DONE
    =techno1
        <color=red>This button is for a basic attack! Dealing a small amount of damage and inflicting the bleeding status effect</color>
        -> DONE
    =techno2
        <color=red>This button is for your skills! Use blood to perform special attacks on your opponents</color>
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
        <color=red>Technoblade doesn't die once his hp drops to 0</color>
        Instead the blood meter will used for his health instead
        He will then only die once his blood reaches 0
        ->DONE
    =E1
        ~displayPortrait("Technoblade", "default")
        Chat's actually being useful for a change I'm even learning things I didn't know about myself
        ~displayPortrait("empty", "default")
        <color=red>E</color>
        ~displayPortrait("Technoblade", "default")
        . . . 
        ~displayPortrait("empty", "default")
        <color=red>EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE</color>
        ~displayPortrait("Technoblade", "default")
        I regret saying anything positive about the chat
    ->DONE
== wilbur ==
        #playable
        0
        ~displayPortrait("Technoblade", "default")
        ~setTextSound("technoblade")
        Wilbur?
        ~displayPortrait("Wilbur", "surprised")
        ~setTextSound("Wilbur")
        oh hey, it's THE blade
        ~displayPortrait("Wilbur", "default")
        are you here to worship the
        great
        almighty
        Penultimate
        Pyramid
        ~displayPortrait("Technoblade", "default")
        ~setTextSound("technoblade")
        No, I heard there's free cookies
        ~displayPortrait("Wilbur", "kindaangry")
        ~setTextSound("Wilbur")
        free cookies???
        technoblade, this structure is so much more then just a place where you can get cookies.
        Can't you understand, its THE 
        great
        almighty
        Penultimate
        Holy
        Pyramid
        ~displayPortrait("Technoblade", "default")
        ~setTextSound("technoblade")
        Wilbur, have you even been in the pyramid?
        ~displayPortrait("Wilbur", "default")
        ~setTextSound("Wilbur")
        Me? No I haven't
        But that's not important. The moment I saw it 20 minutes ago, I knew it was special.
        One day may it open its magnificent locked doors for all to trensend into the next world
        ~displayPortrait("Technoblade", "default")
        ~setTextSound("technoblade")
        Hmm, I can't believe I didn't notice this before
        ~displayPortrait("Wilbur", "choatic")
        ~setTextSound("Wilbur")
        YEESSSSS, TECHNOBLADE. Now can you truly see the greater meaning of the Pyramid.
        ~displayPortrait("Technoblade", "default")
        ~setTextSound("technoblade")
        No Wilbur it isn't that
        I found a key that says its to open up the pyramid
        ~displayPortrait("Wilbur", "silent")
        ~setTextSound("Wilbur")
        ...
        ~displayPortrait("Technoblade", "default")
        ~setTextSound("technoblade")
        What, did I say something wrong?
        ~displayPortrait("Wilbur", "silent")
        ~setTextSound("Wilbur")
        Technoblade, I'm going to need those keys
        #battle
        0
        ~displayPortrait("Technoblade", "default")
        ~setTextSound("technoblade")
        If you want to go so bad, you could just follow me in
        ~displayPortrait("Wilbur", "disgusted")
        ~setTextSound("Wilbur")
        Its not that man
        The problem is that my savior is a pig who has quite literally gone on the record on being an atheist
        It just doesn't sit right
        ~displayPortrait("Technoblade", "default")
        ~setTextSound("technoblade")
        would a cookies help?
        ~displayPortrait("Wilbur", "disgusted")
        ~setTextSound("Wilbur")
        you've ruined my appetite, JUST GO!
        -> DONE