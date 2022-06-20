// (c) Copyright HutongGames, LLC 2010-2021. All rights reserved.

using System;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

namespace HutongGames.PlayMaker.ChainSafe
{
    [ActionCategory("ChainSafe")]
    [Tooltip("Get all 721 tokens from an account")]
    public class ReadERC721All : FsmStateAction
    {
        [Tooltip("Blockchain")]
        [RequiredField]
        public FsmString chain;

        [Tooltip("Network")]
        [RequiredField]
        public FsmString network;

        [Tooltip("Account address")]
        [RequiredField]
        public FsmString account;

        [Tooltip("Optional: Smart contract address")]
        public FsmString contract;

        [RequiredField]
        public FsmInt first;

        [RequiredField]
        public FsmInt skip;

        [Tooltip("Optional: Custom RPC")]
        public FsmString rpc;

        [RequiredField]
        public FsmEvent successEvent;

        [RequiredField]
        public FsmEvent failEvent;

        [UIHint(UIHint.Variable)]
        [Tooltip("Output balance as a string")]
        [RequiredField]
        public FsmString uri;


        public override void Reset()
        {
            chain = null;
            network = null;
            contract = null;
            rpc = null;
            uri = null;
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
            string response = String.Empty;

            // if (rpc != null)
            // {
            //   response = await EVM.AllErc721(chain.Value, network.Value, contract.Value, contract.Value, first.Value, skip.Value, rpc.Value);
            // }
            // else
            // {
            //     response = await EVM.AllErc721(chain.Value, network.Value, contract.Value, contract.Value, first.Value, skip.Value);
            // }

           
            response = await EVM.AllErc721(chain.Value, network.Value, contract.Value, contract.Value, first.Value, skip.Value);

            
            
            if (string.IsNullOrEmpty(response))
            {
                Fsm.Event(failEvent);
            }

            if (this.uri != null) this.uri.Value = response;

            Fsm.Event(successEvent);
        }
    }
}