using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketManager
{

    public static TicketManager main {get; private set;} = new TicketManager();


    public int ticketBalance {get; private set;}
    public bool hasRetrievedInitialAmount {get; private set;}

    public void RetrieveInitialAmount(PlayerData data){
        ticketBalance = data.ticketCount;
        hasRetrievedInitialAmount = true;
    }

    public bool MakePurchase(int cost){
        if(ticketBalance >= cost && hasRetrievedInitialAmount){
            ticketBalance -= cost;
            return true;
        }
        return false;
    }

    public bool AddToBalance(int amountToAdd){
        ticketBalance += amountToAdd;
        return true;
    }

    private TicketManager(){
        hasRetrievedInitialAmount = false;
        ticketBalance = 0;
    }
}
