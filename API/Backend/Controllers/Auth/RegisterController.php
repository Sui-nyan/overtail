<?php

class RegisterController extends Controller
{
	protected array $paths = ['/register'];
	protected array $methods = ['POST'];
	protected array $reqVar = ['email', 'password'];

	protected function execute(): void {
		$email = IO::POST('email');

		if (!filter_var($email, FILTER_VALIDATE_EMAIL))		// Check if input is a valid email
			(new ErrorView(400))->render();

		do {
			$uuid = Utils::uuid();
		} while (intval((new Query('SELECT count(`uuid`) count FROM `User` WHERE `uuid`=:uuid;', [':uuid' => $uuid]))->fetch()['count']) > 0);
		$act = hash('xxh3', $uuid);							// Generate unique activation token (with fastest but insecure hash)

		$success = false;
		try {
			$success = (new Query(							// Insert user into database
				'INSERT INTO `User` (`uuid`, `email`, `password`, `activation`) VALUES (:uuid, :email, :pass, :act);',
				[
					':uuid' => $uuid,
					':email' => $email,
					':pass' => password_hash(IO::POST('password'), PASSWORD_DEFAULT),
					':act' => $act
				]
			))->success();

			if ($success) {
				$message = 'Welcome to Overtail, please <a href="'.DOMAIN.'/activate/'.urlencode($act).'">click here</a> activate your account.';
				$mail = new Mail($email, 'Account activation', $message);
				$success = $mail->send();

				if (!$success)
					(new Query('DELETE FROM `User` WHERE `uuid`=:uuid;', [':uuid' => $uuid]))->count();
			}
		} catch (PDOException) {							// Insert query failed bc of e. g. unique constraints
			// Check if an account with the email exists already
			if ((new Query('SELECT `uuid` FROM `User` WHERE `email`=:email;', [':email' => $email]))->count() > 0) {
				(new ErrorView(486))->render();				// Custom error code (E-Mail already taken)
				return;
			}
		}
		(new APIView(['success' => $success]))->render();
	}
}
