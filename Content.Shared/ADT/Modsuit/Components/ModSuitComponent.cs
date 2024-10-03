using Content.Shared.Inventory;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.ADT.ModSuits;

/// <summary>
///     This component gives an item an action that will equip or un-equip some clothing e.g.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ModSuitComponent : Component
{
    public const string DefaultClothingContainerId = "modsuit-part";

    /// <summary>
    ///     Action used to toggle the clothing on or off.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntProtoId Action = "ActionToggleSuitPiece";

    [DataField, AutoNetworkedField]
    public EntityUid? ActionEntity;

    /// <summary>
    ///     Dictionary of inventory slots and entity prototypes to spawn into the clothing container.
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public Dictionary<string, EntProtoId> ClothingPrototypes = default!;

    /// <summary>
    ///     Dictionary of clothing uids and slots
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<EntityUid, string> ClothingUids = new();

    /// <summary>
    ///     The inventory slot flags required for this component to function.
    /// </summary>
    [DataField("requiredSlot"), AutoNetworkedField]
    public SlotFlags RequiredFlags = SlotFlags.OUTERCLOTHING;

    /// <summary>
    ///     The container that the clothing is stored in when not equipped.
    /// </summary>
    [DataField, AutoNetworkedField]
    public string ContainerId = DefaultClothingContainerId;

    [ViewVariables]
    public Container? Container;

    /// <summary>
    ///     Time it takes for this clothing to be toggled via the stripping menu verbs. Null prevents the verb from even showing up.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan? StripDelay = TimeSpan.FromSeconds(3);

    /// <summary>
    ///     Text shown in the toggle-clothing verb. Defaults to using the name of the <see cref="ActionEntity"/> action.
    /// </summary>
    [DataField, AutoNetworkedField]
    public string? VerbText;

    /// <summary>
    ///     If true it will block unequip of this entity until all attached clothing are removed
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool BlockUnequipWhenAttached = true;

    /// <summary>
    ///     If true all attached will replace already equipped clothing on equip attempt
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool ReplaceCurrentClothing = true;

    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public int ToggleClothingDoAfter = 7;
}

[Serializable, NetSerializable]
public enum ToggleModSuitUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class ModSuitUiMessage : BoundUserInterfaceMessage
{
    public NetEntity AttachedClothingUid;

    public ModSuitUiMessage(NetEntity attachedClothingUid)
    {
        AttachedClothingUid = attachedClothingUid;
    }
}