// (c) Copyright HutongGames, LLC 2010-2021. All rights reserved.

using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

namespace HutongGames.PlayMaker.ChainSafe
{
    [ActionCategory("ChainSafe")]
    [Tooltip("Get total ERC20 token balance by account")]
    public class ReadERC20BalanceOf : FsmStateAction
    {
        [Tooltip("Blockchain")]
        [RequiredField]
        public FsmString chain;

        [Tooltip("Network")]
        [RequiredField]
        public FsmString network;

        [Tooltip("Smart contract address")]
        [RequiredField]
        public FsmString contract;

        [Tooltip("Account address")]
        [RequiredField]
        public FsmString account;

        [Tooltip("Optional: Custom RPC")]
        public FsmString rpc;

        [RequiredField]
        public FsmEvent successEvent;

        [RequiredField]
        public FsmEvent failEvent;

        [UIHint(UIHint.Variable)]
        [Tooltip("Output balance as a string")]
        [RequiredField]
        public FsmString balance;


        public override void Reset()
        {
            chain = null;
            network = null;
            contract = null;
            rpc = null;
            account = null;
            balance = null;
            successEvent = null;
            failEvent = null;
        }

        public override void OnEnter()
        {
#pragma warning disable CS4014
            DoGetData();
#pragma warning restore CS4014
        }

        private async Task DoGetData()
        {
            BigInteger balanceOf = -1;

            if (rpc != null)
            {
                balanceOf = await ERC20.BalanceOf(chain.Value, network.Value, contract.Value, account.Value, rpc.Value);
            }
            else
            {
                balanceOf = await ERC20.BalanceOf(chain.Value, network.Value, contract.Value, account.Value);
            }

            if (balanceOf == -1)
            {
                Fsm.Event(failEvent);
            }

            if (balance != null) balance.Value = balanceOf.ToString();

            Fsm.Event(successEvent);
        }
    }
}