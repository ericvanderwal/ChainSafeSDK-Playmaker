// (c) Copyright HutongGames, LLC 2010-2021. All rights reserved.

using System;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

namespace HutongGames.PlayMaker.ChainSafe
{
    [ActionCategory("ChainSafe")]
    [Tooltip("Get ERC721 (NFT) metadata uri")]
    public class ReadERC721Uri : FsmStateAction
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
        
        [Tooltip("NFT token ID number")]
        [RequiredField]
        public FsmInt tokenId;

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
            string uri = String.Empty;

            if (rpc != null)
            {
                uri = await ERC721.URI(chain.Value, network.Value, contract.Value, tokenId.Value.ToString(), rpc.Value);
            }
            else
            {
                uri = await ERC721.URI(chain.Value, network.Value, contract.Value, tokenId.Value.ToString());
            }

            if (string.IsNullOrEmpty(uri))
            {
                Fsm.Event(failEvent);
            }

            if (this.uri != null) this.uri.Value = uri;

            Fsm.Event(successEvent);
        }
    }
}