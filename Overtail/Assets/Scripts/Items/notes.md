# Latest...

Think `Minecraft`

Current Draft:

## `Item`/`ItemStack`

---

`Item` besteht aus (composition) `IItemComponents` und enthaellt sonst nur `ID` und `Name`
`ItemStack` ist ein Datenpaar `Item` + `Quantity` (dein `ItemMeta`)

## `IItemComponent`

Komponenten gibts zurzeit:
- Equip (definiert dass es Anziehbar ist, und hat Stats als werte)
- Use (ob benutzbar Kampf und roaming state/overworld). Hat eine weitere `ConsumedOnUse` eigenschaft.
- Trash (Ob etwas trashbar ist)
- Stackable (ob etwas stapelbar ist.)

---

## `ItemContainer` (Inventory)

Inventory ist allgemeiner ein `ItemContainer` , einfach eine collection für `ItemStack`
Hier sind nur `Add`, `Remove` definiert und was passiert bei `ItemStack.quantity == 0` (Aus der collection rauswerfen).
Was noch dazu kommt ist `Sort`

---

## `IItemSystem`

... systeme/logik für die komponenten. Ein System je Komponente. 
Die einzelnen klassen sind so kurz... vielleicht kann man es auch einfach zusammenfassen direkt
Die Methoden nehmen `ItemStack` statt `Item` als parameter :thinking:  ist irgendwie awkward, aber ich wusste nicht was sonst.
Zusaetzlich `Player` oder `ItemContainer` um zB Dinge anzuziehen oderso.
Unter anderem gibts immer eine methode `IsCompatible(ItemStack)`, um einfach abzufragen ob das Item eine gewisse option zur verfuegung hat (zB wenn wir UI optionen erstellen)

---

## `Inventorymanager`

... ist einfach ein 'master interface' für all die systeme.
Es hat zusaetzlich felder für `Player` und `Inventory` im konstruktor. Dauerreferenz.

Es ruft die einzelnen Systeme auf und ueberreicht relevante parameter

---
## `InventoryGUI`

... würde dann praktisch eine Instanz von `InventoryManager` haben lediglich und von da alles abfragen


- Welche Items sind available?
- Welche Optionen hat jedes Item
- Was passiert bei welcher Option.





# Previously...



# What else

What else could we possibly want?
Any of the following properties belonging to ONLY ONE component?
- On attack extra special effects
- Droppable property(Quest/Key items cant be dropped)
- amount(100 Cat ears, potions, ammo)
- consumed property(potions should be consumed on use)




## On-attack effects
### Contact points with other components inside same Item
- Separate stats? or weapon stats?
### Other contact points
- Takes stats from user? (Would need to access user)
- Stats from other equipment?
- Set effects? (Could be own`SetComponent`)

## Droppable/Throw away
### Other components
- Drop variable `Amount`
### Other
- None

## Amount
### other components
- `Consumed` property (consequently indirectly all `use` type components)
### Other
- Possibly quests/achievements.

## Consumed
### other components
- Interface between all `Use`type components and `Amount`
- => If there is no way to `use` an item, the `consumed` property becomes redundant
### Other
- None





# Some thoughts

## Why not `Consumed + Amount`

Possible combinations:

- `consumable with amount > 1` potions, ammunition
- `consumable with amount 1` non stackable, something important/large. Signified by its own item slot
- `not consumed with amount 1` Reusable item. Bike, lighter, wet stone. Tools.
- `not consumed with amount > 1` Collectibles. "Collect 5 X" Quests or Achievements (100 nekomimi cat ears around the island)

## `Consumed` into `Use`?

`Consumed` property could be tied to its respective `Use` component

- `Use` components need access to `Amount`
- Different `Use` behaviour?



## `Droppable` is just one `bool`
- ... yeah hmhm
