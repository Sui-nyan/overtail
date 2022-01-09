<?php

use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\Exception;

class Mail
{
	function __construct(
		private string $to,
		private string $title,
		private string $message,
	){}

	public function send(): bool {
		$mail = new PHPMailer(true);									// Create an instance; passing `true` enables exceptions

		try {
			// Server settings
			$mail->isSMTP();											// Send using SMTP
			$mail->Host       = 'smtp.strato.de';						// Set the SMTP server to send through
			$mail->SMTPAuth   = true;									// Enable SMTP authentication
			$mail->Username   = 'overtail@schindlerfelix.de';			// SMTP username
			$mail->Password   = 'VCgR=5v?!Gs__FpB';						// SMTP password
			$mail->SMTPSecure = PHPMailer::ENCRYPTION_SMTPS;			// Enable implicit TLS encryption
			$mail->Port       = 465;									// TCP port to connect to; use 587 if you have set `SMTPSecure = PHPMailer::ENCRYPTION_STARTTLS`

			// Recipients
			$mail->setFrom('overtail@schindlerfelix.de', 'Overtail');	// Set from name
			$mail->addAddress($this->to);								// Add a recipient

			$mailTemplate = file_get_contents(__DIR__ . '/mail.html');	// Load HTML template
			$body = str_replace('% TITLE %', TITLE." &middot; {$this->title}", $mailTemplate);	// Insert title
			$body = str_replace('% BODY %', $this->message, $body);		// Insert message

			// Content
			$mail->isHTML(true);                                        // Set email format to HTML
			$mail->Subject = $this->title;
			$mail->Body    = $body;
			$mail->AltBody = $this->message;

			$mail->send();
			return true;
		} catch (Exception) {
			return false;
		}
	}
}
