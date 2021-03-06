﻿using UnityEngine;
using System.Collections;

/******************************************************************************
* EnemyRocket */
/** 
* Represents Rockets that are launched by the enemy.
******************************************************************************/
public class EnemyRocket : Rocket
  {
  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  /****************************************************************************
  * Start */ 
  /**
  ****************************************************************************/
  public override void Start()
    {
    base.Start();

    setWeaponSpeed((float)Random.Range(10, 20));
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
      if(coll.gameObject.tag == "PlayerRocket")
        {
        MainGame.incrementPlayerScore(directHitValue);
        }
     
      /** Increase player score when hit by player's Explosion. **/
      else if(coll.gameObject.tag == "PlayerExplosion")
        {
        MainGame.incrementPlayerScore(proximityHitValue);
        }
      
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
  * targetBuilding */ 
  /**
  * Targets a building on the screen.
  ****************************************************************************/
  public void targetBuilding()
    {
    //Vector3 targetPosition = new Vector3(Random.Range(minX, maxX), minY, z);

    mTarget = findTargetBuilding();

    mScreenPoint           = transform.localPosition;
    mOffset                = new Vector2(mTarget.x - mScreenPoint.x, mTarget.y - mScreenPoint.y);
    mAngle                 = Mathf.Atan2(mOffset.y, mOffset.x) * Mathf.Rad2Deg;
    transform.rotation     = Quaternion.Euler(0, 0, mAngle);
    }

  /****************************************************************************
  * launchFromRandomPoint */ 
  /**
  * Launch rocket from random point.
  ****************************************************************************/
  public void launchFromRandomPoint()
    {
    /** Find random position, and set as target. */
    transform.position = new Vector3(Random.Range(minX, maxX), maxY, z);
    targetBuilding();
    }

  /****************************************************************************
  * launchFromPoint */ 
  /**
  * Launch rocket from fixed point. Allow for speed change in case it is
  * needed (MIRVs utilize this).
  * @param  launchFrom  Point to launch from.
  * @param  newSpeed    New speed factor.
  ****************************************************************************/
  public void launchFromPoint(Vector3 launchFrom, float newSpeed)
    {
    if(speed != newSpeed && newSpeed > 0)
      setWeaponSpeed(newSpeed);
    
    transform.position = launchFrom;
    targetBuilding();
    }

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
      /** Destroy rocket if it did not hit anything and reaches target point. */
      if(transform.position == mTarget)
        playExplosionAnim();
      else
        {
        mStep              = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, mTarget, mStep);
        }
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
    base.playExplosionAnim();
    gameObject.layer = MainGame.enemyExplosionLayer;
    gameObject.tag   = MainGame.enemyExplosionTag;
    }
  }
