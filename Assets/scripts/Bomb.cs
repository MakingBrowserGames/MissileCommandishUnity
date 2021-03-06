﻿using UnityEngine;
using System.Collections;

/******************************************************************************
* Bomb */
/** 
* A bomb that drops on targets and is created by the BlueBomber.
******************************************************************************/
public class Bomb : Weapon
  {
  public AudioClip bombExplosion;

  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  /****************************************************************************
  * Start */ 
  /**
  ****************************************************************************/
  public override void Start ()
    {

    setWeaponSpeed((float)Random.Range(6, 10));
	  }
	
  /****************************************************************************
  * Update */ 
  /**
  ****************************************************************************/
  public override void Update ()
    {
    base.Update();
    move();
    tryDestroy();
	  }

  /****************************************************************************
  * OnCollisionEnter2D */ 
  /**
  ****************************************************************************/
  void OnCollisionEnter2D(Collision2D coll)
    {
    /** Increase playerScore. */
    if (!dead)
      {
      /** Increase player score when hit by player's Rocket. **/
      if (coll.gameObject.tag == "PlayerRocket")
        MainGame.incrementPlayerScore(directHitValue);

      /** Increase player score when hit by player's Explosion. **/
      else if (coll.gameObject.tag == "PlayerExplosion")
        MainGame.incrementPlayerScore(proximityHitValue);

      /** Hit the ground. */
      else if (coll.gameObject.tag == "Ground")
        MainGame.incrementShakeCounter(0.5f);

      playExplosionAnim();
      }
    }

  /****************************************************************************
  * OnParticleCollision */ 
  /**
  ****************************************************************************/
  void OnParticleCollision(GameObject other)
    {
    /** Increase player score when hit by player's Explosion. **/
    if (!dead)
      {
      MainGame.incrementPlayerScore(particleHitValue);
      playExplosionAnim();
      }
    }

  /****************************************************************************
  * Methods 
  ****************************************************************************/
  /****************************************************************************
  * move */ 
  /**
  * Moves the rocket if not dead.
  ****************************************************************************/
  public override void move()
    {
    /** Update if the enemy class is enabled (game not paused). */
    if(MainGame.FindObjectOfType<Enemy>().enabled && !dead)
      {
      mStep              = speed * Time.deltaTime;
      transform.position = Vector3.MoveTowards(transform.position, mTarget, mStep);
      }

    if(transform.position == mTarget && !dead)
      {
      playExplosionAnim();
      }
    }
  
  /****************************************************************************
  * playExplosionAnim */ 
  /**
  * Plays the Explosion Animation. Sets the layer to the EnemyExplosion layer.
  * and Tag.
  ****************************************************************************/
  public override void playExplosionAnim()
    {
    dead = true;

    GetComponent<AudioSource>().clip    = bombExplosion;
    GetComponent<AudioSource>().enabled = true;
    GetComponent<AudioSource>().Play();

    Animator a = this.GetComponent<Animator>();
    a.Play("BombExplosion");
    gameObject.layer = MainGame.enemyExplosionLayer;
    gameObject.tag   = MainGame.enemyExplosionTag;
    }

  /****************************************************************************
  * setTarget */ 
  /**
  * Sets the target of the bomb.
  ****************************************************************************/
  public void setTarget(Vector3 target)
    {
    target.y -= 10;
    mTarget = target;
    }
  
  /****************************************************************************
  * tryDestroy */ 
  /**
  * Checks if the animation is finished, "Done" state, and destroys the object.
  ****************************************************************************/
  public override void tryDestroy()
    {
    if (dead)
      {
      if (checkAnimDone ())
        {
        Destroy (gameObject);
        }
      }
    }
  }
