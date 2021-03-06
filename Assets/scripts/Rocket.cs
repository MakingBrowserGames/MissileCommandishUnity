﻿using UnityEngine;
using System.Collections;

/******************************************************************************
* Rocket */
/** 
* Base class for Rocket weapons.
******************************************************************************/
public abstract class Rocket : Weapon
  {
  public    ParticleSystem rocketFirePS;
  public    ParticleSystem rocketExplostionPS;

  public    AudioClip      rocketExplosion1;
  public    AudioClip      rocketExplosion2;
  public    AudioClip      rocketExplosion3;
  public    AudioClip      rocketExplosion4;
  public    AudioClip      rocketLaunch1;
  public    AudioClip      rocketLaunch2;
  public    AudioClip      rocketLaunch3;

  protected Vector2        mOffset;
  protected AudioClip[]    mRocketExplosions;
  protected AudioClip[]    mRocketLaunches;

  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  /****************************************************************************
  * Start */ 
  /**
  ****************************************************************************/
  public override void Start()
    {
    setWeaponSpeed(10.0f);
    dead = false;
    mRocketExplosions = new AudioClip[] {rocketExplosion1, rocketExplosion2, rocketExplosion3, rocketExplosion4};
    mRocketLaunches   = new AudioClip[] {rocketLaunch1,    rocketLaunch2,    rocketLaunch3};
    }

  /****************************************************************************
  * Update */ 
  /**
  ****************************************************************************/
  public override void Update()
    {
    base.Update();
    }

  /****************************************************************************
  * Methods 
  ****************************************************************************/
  /****************************************************************************
  * move */ 
  /**
  * Must be overridden.
  ****************************************************************************/
  public override void move(){ }
  
  /****************************************************************************
  * playExplosionAnim */ 
  /**
  * Plays the Explosion Animation.
  ****************************************************************************/
  public override void playExplosionAnim()
    {
    if (!dead)
      {
      dead = true;
      if (rocketExplostionPS != null)
        rocketExplostionPS.Play();
      
      stopAudioSource();
      this.GetComponent<Animator>().Play("Explosion");
      playRocketExplosionSound();
      }
    }

  /****************************************************************************
  * playRocketExplosionSound */ 
  /**
  * Plays the explosion sound of the Rocket.
  ****************************************************************************/
  public void playRocketExplosionSound()
    {
    if (GetComponent<AudioSource>() != null)
      {
      int r = Random.Range(0, 3);
      GetComponent<AudioSource>().clip    = mRocketExplosions[r];
      GetComponent<AudioSource>().enabled = true;
      GetComponent<AudioSource>().Play();
      }
    }

  /****************************************************************************
  * playRocketLaunchSound */ 
  /**
  * Plays the launch sound of the Rocket.
  ****************************************************************************/
  public void playRocketLaunchSound()
    {

    if (GetComponent<AudioSource>() != null)
      {
      int r = Random.Range(0, 2);
      GetComponent<AudioSource>().clip    = mRocketLaunches[r];
      GetComponent<AudioSource>().enabled = true;
      GetComponent<AudioSource>().Play();
      }
    }
  
  /****************************************************************************
  * setTarget */ 
  /**
  * Sets the target for the rocket to fly to.
  *
  * @param  target  Point in the world to fly to.
  ****************************************************************************/
  public void setTarget(Vector3 target)
    {
    mTarget   = target;
    mTarget.z = z;
    }

  /****************************************************************************
  * tryDestroy */ 
  /**
  * Checks if the animation is finished, "Done" state, and destroys the object.
  ****************************************************************************/
  public override void tryDestroy()
    {
    base.tryDestroy();
    }
  }