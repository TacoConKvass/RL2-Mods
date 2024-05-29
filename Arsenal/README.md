# A few words about the configs AppliesToClasses
The saved values are values of an enum, so they get converted to numbers.
Unfortunately that is not easily readable, so here is the list of what number corresponds to which class
```c#
Knight = 10,
Barbarian = 20,
Mage = 30,
Assassin = 40,
Valkyrie = 70,
Ranger = 90,
Duelist = 130,
Chef = 140,
Boxer = 150,
Dragon Lancer = 160,
Gunslinger = 170,
Ronin = 180,
Pirate = 190,
Bard = 200,
Astromancer = 210,
```
If the same class is mentioned in different configs (for example it's in both WeaponsOnly and TalentsOnly) the closest setting to the bottom will be applied.