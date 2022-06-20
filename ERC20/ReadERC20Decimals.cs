// (c) Copyright HutongGames, LLC 2010-2021. All rights reserved.

using System;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

namespace HutongGames.PlayMaker.ChainSafe
{
    [ActionCategory("ChainSafe")]
    [Tooltip("Get total ERC20 token decimal points")]
    public class ReadERC20Decimals : FsmStateAction
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

        [Tooltip("Optional: Custom RPC")]
        public FsmString rpc;

        [RequiredField]
        public FsmEvent successEvent;

        [RequiredField]
        public FsmEvent failEvent;

        [UIHint(UIHint.Variable)]
        [Tooltip("Output balance as a string")]
        [RequiredField]
        public FsmString decimals;


        public override void Reset()
        {
            chain = null;
            network = null;
            contract = null;
            rpc = null;
            decimals = null;
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
            BigInteger _decimals = -1;

            if (rpc != null)
            {
                _decimals = await ERC20.Decimals(chain.Value, network.Value, contract.Value, rpc.Value);
            }
            else
            {
                _decimals = await ERC20.Decimals(chain.Value, network.Value, contract.Value);
            }

            if (_decimals == -1)
            {
                Fsm.Event(failEvent);
            }

            if (decimals != null) decimals.Value = _decimals.ToString();

            Fsm.Event(successEvent);
        }
    }
}