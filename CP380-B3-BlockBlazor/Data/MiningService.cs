using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CP380_B1_BlockList.Models;

namespace CP380_B3_BlockBlazor.Data
{
    public class MiningService
    {
        private readonly BlockService bService;
        private readonly PendingTransactionService _pendingTransactionService;
        public IEnumerable<Block> TmpBkList { get; set; }
        public IEnumerable<Payload> TmpPddList { get; set; }

        public MiningService(BlockService blockService, PendingTransactionService pendingTransactionService)
        {
            bService = blockService;
            _pendingTransactionService = pendingTransactionService;
        }

        public async Task<Block> MineBlock()
        {

            TmpBkList = await bService.ListBlocks();
            TmpPddList = await _pendingTransactionService.ListPayloads();

            new Block(DateTime.Now, TmpBkList.Select(b => b.PreviousHash).Last(), TmpPddList.ToList()).Mine(2);

            return new Block(DateTime.Now, TmpBkList.Select(b => b.PreviousHash).Last(), TmpPddList.ToList());

        }
    }
}