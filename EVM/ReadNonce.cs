// (c) Copyright HutongGames, LLC 2010-2021. All rights reserved.

using System;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

namespace HutongGames.PlayMaker.ChainSafe
{
    [ActionCategory("ChainSafe")]
    [Tooltip("Get Nonce")]
    public class ReadNonce : FsmStateAction
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

        [Tooltip("Optional: Custom RPC")]
        public FsmString rpc;

        [RequiredField]
        public FsmEvent successEvent;

        [RequiredField]
        public FsmEvent failEvent;

        [UIHint(UIHint.Variable)]
        [Tooltip("Output nonce as a string")]
        [RequiredField]
        public FsmString nonce;


        public override void Reset()
        {
            chain = null;
            network = null;
            rpc = null;
            nonce = null;
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
            string _nonce = String.Empty;

            if (rpc != null)
            {
                _nonce = await EVM.Nonce(chain.Value, network.Value, account.Value, rpc.Value);
            }
            else
            {
                _nonce = await EVM.Nonce(chain.Value, network.Value, account.Value);
            }

            if (string.IsNullOrEmpty(_nonce))
            {
                Fsm.Event(failEvent);
            }

            if (nonce != null) nonce.Value = _nonce;

            Fsm.Event(successEvent);
        }
    }
}