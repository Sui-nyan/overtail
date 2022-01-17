<?php

class SaveInvController extends Controller
{
	protected array $paths = ['/inv/save', '/inventory/save'];
	protected array $methods = ['POST'];
	protected array $reqVar = ['invData'];

	protected function execute(): void {
		$invData = json_decode(IO::POST("invData"));
		$uuid = Auth::tokenUuid();
		$q = new Query('DELETE FROM `Inventory` WHERE `uuid`=:uuid;', [':uuid' => $uuid]);

		if ($q->success() && $invData != null) {		// If old data was deleted successfully and there is
			foreach ($invData as $item) {				// new data, insert the new one (Item by Item)
				$q = new InsertQuery('Inventory');
				$q->add('uuid', $uuid);
				$q->add('item', $item['item']);
				$q->add('amount', $item['amount']);
				$q->run();
			}
		}
	}
}
