// (c) Copyright HutongGames, LLC 2010-2021. All rights reserved.

using System;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

namespace HutongGames.PlayMaker.ChainSafe
{
    [ActionCategory("ChainSafe")]
    [Tooltip("Get transaction status")]
    public class ReadTransactionStatus : FsmStateAction
    {
        [Tooltip("Blockchain")]
        [RequiredField]
        public FsmString chain;

        [Tooltip("Network")]
        [RequiredField]
        public FsmString network;

        [Tooltip("Account address")]
        [RequiredField]
        public FsmString transaction;

        [Tooltip("Optional: Custom RPC")]
        public FsmString rpc;

        [RequiredField]
        public FsmEvent successEvent;

        [RequiredField]
        public FsmEvent failEvent;

        [UIHint(UIHint.Variable)]
        [Tooltip("Returns transaction status. May return success, fail or pending.")]
        [RequiredField]
        public FsmString status;


        public override void Reset()
        {
            chain = null;
            transaction = null;
            network = null;
            rpc = null;
            status = null;
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
            string _status = String.Empty;

            if (rpc != null)
            {
                _status = await EVM.TxStatus(chain.Value, network.Value, transaction.Value, rpc.Value);
            }
            else
            {
                _status = await EVM.TxStatus(chain.Value, network.Value, transaction.Value);
            }

            if (string.IsNullOrEmpty(_status))
            {
                Fsm.Event(failEvent);
            }

            if (status != null) status.Value = _status;

            Fsm.Event(successEvent);
        }
    }
}