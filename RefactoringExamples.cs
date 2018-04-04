// REFACTORING EXAMPLES

/*
	find situations where you're repeating lines of code.
	break it into a separate method instead.
*/

// BAD
void OnTriggerEnter2D (Collider2D collider) {
	if (collider.gameObject.tag == "Enemy") {
		health -= 10;
		hasBeenHurt = true;
	}
	else if (collider.gameObject.tag == "Spikes") {
		health -= 100;
		hasBeenHurt = true;
	}
	else if (collider.gameObject.tag == "Bullet") {
		health -= 5;
		hasBeenHurt = true;
	}
	else if (collider.gameObject.tag == "Lava") {
		health -= 50;
		hasBeenHurt = true;
	}
}




// BETTER
void OnTriggerEnter2D (Collider2D collider) {
	if (collider.gameObject.tag == "Enemy") {
		ApplyDamage(10);
	}
	else if (collider.gameObject.tag == "Spikes") {
		ApplyDamage(100);
	}
	else if (collider.gameObject.tag == "Bullet") {
		ApplyDamage(5);
	}
	else if (collider.gameObject.tag == "Lava") {
		ApplyDamage(50);
	}
}

void ApplyDamage (int damageAmount) {
	if (damageAmount > 0) {
		health -= damageAmount;
		hasBeenHurt = true;
	}
}




// EVEN BETTERER
void OnTriggerEnter2D (Collider2D collider) {
	ApplyDamage(GetDamageAmount(collider.gameObject.tag));
}

int GetDamageAmount (string tag) {
	switch(tag) {
		case "Enemy":
			return 10;
		case "Spikes":
			return 100;
		case "Bullet":
			return 5;
		case "Lava":
			return 50;
		default:
			return 0;
	}
}

void ApplyDamage (int damageAmount) {
	if (damageAmount > 0) {
		health -= damageAmount;
		hasBeenHurt = true;
	}
}






/*
	remove unnecessary duplicate code in conditional statements
*/

// BAD
void Move () {
	if (isBoosting) {
		speed = baseSpeed * 1.5f;
		ApplyForce();
	}
	else {
		speed = baseSpeed;
		ApplyForce();
	}
}




// BETTER
void Move () {
	if (isBoosting) {
		speed = baseSpeed * 1.5f;
	}
	else {
		speed = baseSpeed;
	}

	ApplyForce();
}






/*
	break long methods into smaller ones that are more specific.
	the more lines there are in a method, the harder it is to understand what it's doing.
*/

// BAD
void ApplyDamage (int damageAmount) {
	if (damageAmount > 0) {
		health -= damageAmount;

		anim.SetTrigger("hurt");
		Sound.instance.PlaySound(hurtClip);

		if (health <= 0) {
			isDead = true;
			gameOverMessage.SetActive(true);
			anim.SetBool("isDead", true);
		}
	}
}




// BETTER
void ApplyDamage (int damageAmount) {
	if (damageAmount > 0) {
		health -= damageAmount;

		HurtEffects();
		CheckForDeath();
	}
}

void HurtEffects () {
	anim.SetTrigger("hurt");
	Sound.instance.PlaySound(hurtClip);
}

void CheckForDeath () {
	if (health <= 0) {
		isDead = true;
		gameOverMessage.SetActive(true);
		anim.SetBool("isDead", true);
	}	
}






/*
	change names of variables and methods to more accurately describe their function
*/

// BAD
Vector3 GetDToP2Hd (bool makeItANormalizedVectorPlease) {
	Vector3 temp = plyr2.GetComponent<Control>().hdPos;
	Vector3 d = temp - transform.position;
	
	if (makeItANormalizedVectorPlease) {
		d.Normalize();
	}

	return d;
}




// BETTER
Vector3 GetDirectionToPlayer2Head (bool normalize) {
	Vector3 headPosition = player2.GetComponent<PlayerController>().headPosition;
	Vector3 result = headPosition - transform.position;

	if (normalize) {
		result.Normalize();
	}

	return result;
}




/*
	too many nested conditional statements makes code hard to understand.
*/

// BAD
float GetPlayerSpeed () {
	float result;

	if (isDead) {
		result = DeadSpeed();
	}
	else {
		if (isHurt) {
			result = HurtSpeed();
		}
		else {
			if (isInQuickSand) {
				result = QuickSandSpeed();
			}
			else {
				if (isOnLand) {
					result = LandSpeed();
				}
				else {
					result = DefaultSpeed();
				}
			}
		}
	}

	return result;
}




// BETTER
float GetPlayerSpeed () {
	if (isDead) {
		return DeadSpeed();
	}

	if (isHurt) {
		return HurtSpeed();
	}

	if (isInQuickSand) {
		return = QuickSandSpeed();
	}

	if (isOnLand) {
		return = LandSpeed();
	}

	return DefaultSpeed();
}






/*
	fix complicated if statements by breaking them out into separate bool variables that describe what’s being checked.
*/

// BAD
void Jump () {
	if (transform.position.y <= groundYPosition &&
		Input.GetKeyDown(KeyCode.Space) &&
		damageManager.stunTimer <= 0) 
	{
		rigidBody.AddForce(Vector3.up * jumpForce, ForeMode.Impulse);
	}
}




// BETTER
void Jump () {
	bool onGround = transform.position.y <= groundYPosition;
	bool jumpPressed = Input.GetKeyDown(KeyCode.Space);
	bool notStunned = damageManager.stunTimer <= 0;

	if (onGround && jumpPressed && notStunned) {
		rigidBody.AddForce(Vector3.up * jumpForce, ForeMode.Impulse);
	}
}






/*
	creating helper scripts to hold common methods used in many other scripts
*/

public class GeneralUtility {	
	public static float RangeToPercentClamp01 (float number, float  min, float max) {
		return Mathf.Clamp01((number - min) / (max - min));
	}

	public static float PercentToRange (float percent, float min, float max) {
		return ((max - min) * percent + min);
	}

	public static Vector3 RandomVector3 () {
		return new Vector3 (Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
	}

	public static Color RGB255 (int r, int g, int b) {
		return new Color (RangeToPercentClamp(r, 0, 255), RangeToPercentClamp(g, 0, 255), RangeToPercentClamp(b, 0, 255));
	}
}





