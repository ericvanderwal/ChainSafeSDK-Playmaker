// (c) Copyright HutongGames, LLC 2010-2021. All rights reserved.

using System;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

namespace HutongGames.PlayMaker.ChainSafe
{
    [ActionCategory("ChainSafe")]
    [Tooltip("Get total ERC20 token name")]
    public class ReadERC20Name : FsmStateAction
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
        public FsmString name;


        public override void Reset()
        {
            chain = null;
            network = null;
            contract = null;
            rpc = null;
            name = null;
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
            string _name = String.Empty;

            if (rpc != null)
            {
                _name = await ERC20.Name(chain.Value, network.Value, contract.Value, rpc.Value);
            }
            else
            {
                _name = await ERC20.Name(chain.Value, network.Value, contract.Value);
            }

            if (string.IsNullOrEmpty(_name))
            {
                Fsm.Event(failEvent);
            }

            if (name != null) name.Value = _name;

            Fsm.Event(successEvent);
        }
    }
}