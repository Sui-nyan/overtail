<?php

class PosSaveController extends Controller
{
	protected array $paths = ['/pos/save', '/position/save'];
	protected array $methods = ['POST'];
	protected array $reqVar = ['x', 'y', 'scene'];
	protected bool $userRequired = true;

	protected function execute(): void {
		$save = new Query('INSERT INTO `PlayerData` (`uuid`, `x`, `y`, `scene`) VALUES (:uuid, :x, :y, :scene) ON DUPLICATE KEY UPDATE `x`=:x, `y`=:y `scene`=:scene;',
			[
				':uuid' => Auth::tokenUuid(),
				':x' => IO::POST('x'),
				':y' => IO::POST('y'),
				':scene' => IO::POST('scene')
			]
		);

		(new APIView(['success' => $save->success()]))->render();
	}
}
