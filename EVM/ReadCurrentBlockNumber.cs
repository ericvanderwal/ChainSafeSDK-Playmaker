// (c) Copyright HutongGames, LLC 2010-2021. All rights reserved.

using System;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

namespace HutongGames.PlayMaker.ChainSafe
{
    [ActionCategory("ChainSafe")]
    [Tooltip("Get the current block number of the blockchain")]
    public class ReadCurrentBlockNumber : FsmStateAction
    {
        [Tooltip("Blockchain")]
        [RequiredField]
        public FsmString chain;

        [Tooltip("Network")]
        [RequiredField]
        public FsmString network;

        [Tooltip("Optional: Custom RPC")]
        public FsmString rpc;

        [RequiredField]
        public FsmEvent successEvent;

        [RequiredField]
        public FsmEvent failEvent;

        [UIHint(UIHint.Variable)]
        [Tooltip("Output balance as a string")]
        [RequiredField]
        public FsmInt blockNumber;


        public override void Reset()
        {
            chain = null;
            network = null;
            rpc = null;
            blockNumber = null;
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
            int _blockNumber = -1;

            if (rpc != null)
            {
                _blockNumber = await EVM.BlockNumber(chain.Value, network.Value, rpc.Value);
            }
            else
            {
                _blockNumber = await EVM.BlockNumber(chain.Value, network.Value);
            }

            if (_blockNumber == -1)
            {
                Fsm.Event(failEvent);
            }

            if (blockNumber != null) blockNumber.Value = _blockNumber;

            Fsm.Event(successEvent);
        }
    }
}