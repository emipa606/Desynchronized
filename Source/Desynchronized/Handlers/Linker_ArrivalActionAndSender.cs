using System.Collections.Generic;
using HugsLib.Utils;
using RimWorld.Planet;
using Verse;

namespace Desynchronized.Handlers;

public class Linker_ArrivalActionAndSender : UtilityWorldObject
{
    private Dictionary<TransportersArrivalAction_GiveGift, int> internalMapping;

    public override void PostAdd()
    {
        base.PostAdd();
        internalMapping = new Dictionary<TransportersArrivalAction_GiveGift, int>();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref internalMapping, "internalMapping", LookMode.Deep, LookMode.Value);
    }

    public void EstablishRelationship(TransportersArrivalAction_GiveGift actionInstance, int senderTileID)
    {
        internalMapping.TryAdd(actionInstance, senderTileID);
    }

    public Map SafelyGetMapOfGivenAction(TransportersArrivalAction_GiveGift actionInstance)
    {
        if (!internalMapping.TryGetValue(actionInstance, out var value))
        {
            return null;
        }

        var result = Current.Game.FindMap(value);
        internalMapping.Remove(actionInstance);
        return result;
    }
}