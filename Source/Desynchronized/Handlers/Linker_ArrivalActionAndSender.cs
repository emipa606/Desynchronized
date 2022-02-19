using System.Collections.Generic;
using HugsLib.Utils;
using RimWorld.Planet;
using Verse;

namespace Desynchronized.Handlers;

public class Linker_ArrivalActionAndSender : UtilityWorldObject
{
    private Dictionary<TransportPodsArrivalAction_GiveGift, int> internalMapping;

    public override void PostAdd()
    {
        base.PostAdd();
        internalMapping = new Dictionary<TransportPodsArrivalAction_GiveGift, int>();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref internalMapping, "internalMapping", LookMode.Deep, LookMode.Value);
    }

    public void EstablishRelationship(TransportPodsArrivalAction_GiveGift actionInstance, int senderTileID)
    {
        if (!internalMapping.ContainsKey(actionInstance))
        {
            internalMapping.Add(actionInstance, senderTileID);
        }
    }

    public Map SafelyGetMapOfGivenAction(TransportPodsArrivalAction_GiveGift actionInstance)
    {
        if (!internalMapping.ContainsKey(actionInstance))
        {
            return null;
        }

        var result = Current.Game.FindMap(internalMapping[actionInstance]);
        internalMapping.Remove(actionInstance);
        return result;
    }
}