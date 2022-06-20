// (c) Copyright HutongGames, LLC 2010-2021. All rights reserved.

using System;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

namespace HutongGames.PlayMaker.ChainSafe
{
    [ActionCategory("ChainSafe")]
    [Tooltip("Get ERC721 (NFT) owners address by token ID")]
    public class ReadERC721OwnerOf : FsmStateAction
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
        public FsmString owner;


        public override void Reset()
        {
            chain = null;
            network = null;
            contract = null;
            rpc = null;
            owner = null;
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
            string _owner = String.Empty;

            if (rpc != null)
            {
                _owner = await ERC721.OwnerOf(chain.Value, network.Value, contract.Value, tokenId.Value.ToString(), rpc.Value);
            }
            else
            {
                _owner = await ERC721.OwnerOf(chain.Value, network.Value, contract.Value, tokenId.Value.ToString());
            }

            if (string.IsNullOrEmpty(_owner))
            {
                Fsm.Event(failEvent);
            }

            if (owner != null) owner.Value = _owner;

            Fsm.Event(successEvent);
        }
    }
}