using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMount : TowerPart
{
    [SerializeField] WeaponMountStyle mountStyle = null;
    [SerializeField] AdvancedTargetingSystem targetingSystem = null;
    [SerializeField] AngleIndicator angleIndicator = null;
    [SerializeField] GameObject rangeIndicator = null;
    [SerializeField] GameObject beam = null;
    [SerializeField] TargetingBeam targetingBeam = null;
    [SerializeField] Transform home = null;
    [SerializeField] GameObject aimPreview = null;
    [SerializeField] bool advancedTargetingSystemActive = false;
    [SerializeField] bool canShootUp = true;
    [SerializeField] bool canShootDown = true;
    [SerializeField] int rotationalSpeed = 1;
    [SerializeField] int range = 1;
    [SerializeField] TurretAngle turretAngle = TurretAngle.A0;
    List<Enemy> enemies = new List<Enemy>();
    SphereCollider detectionRadius;
    Enemy target;
    Weapon[] weapons;
    Vector3 horizontalTargetPosition;
    Vector3 verTargetPos;
    Vector3 currentTargetPosition = Vector3.zero;
    Vector3 previewPosition;
    float turnRadius = 360;
    bool aimingAtHome = false;
    bool previewMode = true;

    public int RotationalSpeed { get => rotationalSpeed; set => rotationalSpeed = value; }
    public int Range { get => range; set => range = value; }
    public TurretAngle TurretAngle { get => turretAngle; set => turretAngle = value; }

    public void SetAccessories(GameObject newHome, GameObject newRangeIndicator, AngleIndicator newAngleIndicator, TargetingBeam newBeam) 
    {
        home = newHome.transform;
        rangeIndicator = newRangeIndicator; 
        angleIndicator = newAngleIndicator; 
        beam = newBeam.gameObject; 
        targetingBeam = newBeam; 
    }
    
    public void InitializeWeaponMount()
    {
        InitializeTurrets();
        SetIndicatorRanges();
        SetTurretAnge();
        previewMode = false;
    }

    protected override void Update()
    {
        base.Update();
        if (!previewMode)
        {
            ManageEnemies();
            if (targetingBeam.EnemyDetected(ref aimPreview)) { Fire(); }
            if (TurretAngle != TurretAngle.A0 && target) { RotateHorizontal(); }
            if (target) if (canShootUp) { RotateVerticle(); }
            SetBeamRotation();
            aimPreview.transform.position = previewPosition;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy != null && !EnemyBeyondTurnRadius(enemy) && !enemies.Contains(enemy)) 
        {
            bool add = false;
            if(enemy.flying && canShootUp) { add = true;}
            else if(!enemy.flying && canShootDown) { add = true;}
            if(add)enemies.Add(enemy); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) { enemies.Remove(enemy); }
    }

    public void SetStyle(WeaponMountStyle style) { mountStyle = style; }

    void InitializeTurrets()
    {
        WeaponMountSlot[] slots = mountStyle.GetSlots();
        weapons = new Weapon[slots.Length];
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i] = slots[i].GetWeapon();
        }
    }

    void SetIndicatorRanges()
    {
        detectionRadius = GetComponent<SphereCollider>();
        detectionRadius.radius = Range;
        angleIndicator.SetAngle(TurretAngle, Range);
        angleIndicator.ShowIndicators(true);
        rangeIndicator.transform.localScale = new Vector3(Range, Range, Range);
        beam.transform.localScale = new Vector3(1, 1, Range * 1.5f);
    }

    void SetTurretAnge()
    {
        switch (TurretAngle)
        {
            case TurretAngle.A0: turnRadius = 0; break;
            case TurretAngle.A45: turnRadius = 45; break;
            case TurretAngle.A90: turnRadius = 90; break;
            case TurretAngle.A135: turnRadius = 135; break;
            case TurretAngle.A180: turnRadius = 180; break;
            case TurretAngle.A225:  turnRadius = 225; break;
            case TurretAngle.A270: turnRadius = 270; break;
            case TurretAngle.A315: turnRadius = 315; break;
            case TurretAngle.A360: turnRadius = 360; break;
        }
    }

    void SetBeamRotation()
    {
        WeaponProjectile weaponBarrel;
        GameObject barrel;
        weaponBarrel = weapons[0].GetComponent<WeaponProjectile>();
        barrel = weaponBarrel.GetBarrel();
        beam.transform.rotation = barrel.transform.rotation;
    }

    void ManageEnemies()
    {
        RemoveNullAndBlindSpotTargets();
        enemies.Sort();
        if (enemies.Count > 0)
        {
            if (enemies[0] != null)
            {
                FindTarget();
            }
        }
    }

    void RemoveNullAndBlindSpotTargets()
    {
        int count = enemies.Count;
        bool foundNull = false;
        int i = 0;
        while (i < count)//remove collection contents while iterating
        {
            for (int j = 0; j < enemies.Count; j++)
            {
                if (enemies[j] == null || EnemyBeyondTurnRadius(enemies[j])) { foundNull = true; }
                if (foundNull)
                {
                    enemies.RemoveAt(j);
                    count = enemies.Count;
                    foundNull = false;
                    break;
                }
            }
            if (!foundNull) {i++;}
        }
    }

    void FindTarget()
    {
        if (!advancedTargetingSystemActive) { target = enemies[0]; }
        else target = targetingSystem.PrioritizeTarget(this, ref enemies);
    }
    
    public bool EnemyBeyondTurnRadius(Enemy enemy)
    {
        if (enemy)
        {
            Quaternion homeRotation = Quaternion.LookRotation(home.transform.position - transform.position);
            float releativeRotationToTarget = Quaternion.LookRotation(enemy.transform.position - transform.position).eulerAngles.y - homeRotation.eulerAngles.y;
            if (releativeRotationToTarget < 0) releativeRotationToTarget = 360 + releativeRotationToTarget;
            if ((releativeRotationToTarget > turnRadius / 2 && releativeRotationToTarget < 360 - (turnRadius / 2))) { return true; }
            else return false;
        }
        return true;
    }

    void Fire()
    {
        if (target)
        {
            foreach(Weapon weapon in weapons) { weapon.Fire(target.transform); }
        }
    }
    
    void RotateHorizontal()
    {
        Rigidbody firePointRB = weapons[0].GetFirePoint().GetComponent<Rigidbody>();
        Rigidbody targetRB = null;
        //float shotSpeed = weapons[0].GetProjectile().GetSpeed();
        Debug.Log("Fix RotateHorizontal Method");
        Vector3 shooterPosition = transform.position;
        Vector3 targetPosition = Vector3.zero;

        ResetRotation(ref targetRB, ref targetPosition);

        Vector3 shooterVelocity = firePointRB ? firePointRB.velocity : Vector3.zero;
        Vector3 targetVelocity = targetRB ? targetRB.velocity : Vector3.zero;

        //SetHorizontalTargetPosition(ref shooterPosition, ref shooterVelocity, ref shotSpeed, ref targetPosition, ref targetVelocity);
        SetHorizontalRotation();
        ContainRotationWithinTurretAngle();
    }

    void SetHorizontalRotation()
    {
        Vector3 newRotation = Vector3.RotateTowards(transform.forward, horizontalTargetPosition, RotationalSpeed * Time.deltaTime, 1);
        Quaternion newNewRotation = new Quaternion();
        newNewRotation.SetLookRotation(newRotation, Vector3.up);
        transform.rotation = newNewRotation;
    }

    void SetHorizontalTargetPosition(ref Vector3 shooterPosition,ref Vector3 shooterVelocity, ref float shotSpeed, ref Vector3 targetPosition, ref Vector3 targetVelocity)
    {
        horizontalTargetPosition = FirstOrderIntercept
                (ref shooterPosition, ref shooterVelocity, ref shotSpeed, ref targetPosition, ref targetVelocity);
        AimHome();
        if (target) previewPosition = new Vector3(horizontalTargetPosition.x, target.transform.position.y, horizontalTargetPosition.z);
        horizontalTargetPosition = horizontalTargetPosition - transform.position;
        currentTargetPosition = horizontalTargetPosition;
        horizontalTargetPosition.y = 0;
        horizontalTargetPosition = horizontalTargetPosition.normalized;
    }

    //return turret to home position to prevent attempts to rotate into blind spot
    void AimHome()
    {
        if (turnRadius < 360 && target)
        {
            if (!EnemyBeyondTurnRadius(target))
            {
                Quaternion homeRotation = Quaternion.LookRotation(home.transform.position - transform.position);
                float releativeRotationToTarget = Quaternion.LookRotation(target.transform.position - transform.position).eulerAngles.y - homeRotation.eulerAngles.y;
                if (releativeRotationToTarget < 0) releativeRotationToTarget = 360 + releativeRotationToTarget;
                float rotationFromHome = transform.localEulerAngles.y;
                //Debug.Log("T " + releativeRotationToTarget + " H " + homeRotation.eulerAngles.y + " H2 " + rotationFromHome);
                if ((releativeRotationToTarget <= 180 && rotationFromHome >= 180) ||
                    (releativeRotationToTarget >= 180 && rotationFromHome <= 180) && (rotationFromHome < -1 || rotationFromHome > 1))
                {//turret will attempt to follow target into blind spot. this corrects that behavior
                    aimingAtHome = true;
                }
                else { aimingAtHome = false; }
            }
        }
    }

    void ResetRotation(ref Rigidbody targetRB, ref Vector3 targetPosition)
    {
        if (!target || aimingAtHome)
        {
            targetPosition = home.position;
            targetRB = new Rigidbody();
        }
        else //return turret to home position to prevent attempts to rotate into blind spot
        {
            targetPosition = target.transform.position;
            targetRB = target.GetComponent<Rigidbody>();
        }
    }

    void ContainRotationWithinTurretAngle()
    {
        if (transform.localEulerAngles.y > turnRadius / 2 && transform.localEulerAngles.y < 360 - (turnRadius / 2))//Barrel rotates into blind spot
        {
            if (transform.localEulerAngles.y > turnRadius / 2 && transform.localEulerAngles.y < 180)//Which side barrel entered blind spot
            {
                transform.localEulerAngles = new Vector3(0, turnRadius / 2, 0); //reset barrel to edge of blind spot
            }
            else { transform.localEulerAngles = new Vector3(0, 360 - (turnRadius / 2), 0); }//reset barrel to edge of blind spot
        }
    }
   
    void RotateVerticle()
    {
        foreach (Weapon weapon in weapons)
        {
            WeaponProjectile weaponBarrel;
            GameObject barrel;
            if ((weaponBarrel = weapon.GetComponent<WeaponProjectile>()) && target != null)
            {
                barrel = weaponBarrel.GetBarrel();
                SetVerticalTargetPosition();
                SetVerticalRotation(ref barrel);
            }
        }
    }

    void SetVerticalTargetPosition()
    {
        verTargetPos = currentTargetPosition;
        currentTargetPosition.y = target.transform.position.y;
        if (!target.flying) verTargetPos.y = -0.5f;
        verTargetPos = verTargetPos.normalized;
    }

    void SetVerticalRotation(ref GameObject barrel)
    {
        Vector3 newRotation = Vector3.RotateTowards(barrel.transform.forward, verTargetPos, RotationalSpeed * Time.deltaTime, 1);
        Quaternion newNewRotation = new Quaternion();
        newNewRotation.SetLookRotation(newRotation, Vector3.up);
        barrel.transform.localRotation = new Quaternion(newNewRotation.x, 0, 0, newNewRotation.w);
    }

    public Vector3 FirstOrderIntercept 
    (ref Vector3 shooterPosition, ref Vector3 shooterVelocity, ref float shotSpeed,ref Vector3 targetPosition, ref Vector3 targetVelocity)
    {
        Vector3 targetRelativePosition = targetPosition - shooterPosition;
        Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
        float t = FirstOrderInterceptTime (ref shotSpeed,ref targetRelativePosition,ref targetRelativeVelocity);
        return targetPosition + t * (targetRelativeVelocity);
    }

    //first-order intercept using relative target position
    public static float FirstOrderInterceptTime(ref  float shotSpeed,ref Vector3 targetRelativePosition, ref  Vector3 targetRelativeVelocity)
    {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if (velocitySquared < 0.001f) return 0f;

        float a = velocitySquared - shotSpeed * shotSpeed;

        //handle similar velocities
        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -targetRelativePosition.sqrMagnitude / (2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition));
            return Mathf.Max(t, 0f); //don't shoot back in time
        }

        float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b * b - 4f * a * c;

        if (determinant > 0f)
        { //determinant > 0; two intercept paths (most common)
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a), t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f) return Mathf.Min(t1, t2); //both are positive
                else return t1; //only t1 is positive
            }
            else return Mathf.Max(t2, 0f); //don't shoot back in time
        }
        else if (determinant < 0f) return 0f; //determinant < 0; no intercept path
        else return Mathf.Max(-b / (2f * a), 0f);//determinant = 0; one intercept path, pretty much never happens //don't shoot back in time
    }
    //The MIT License(MIT)

    //Copyright(c) 2008 Daniel Brauer

    //Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files(the "Software"), 
    //    to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
    //        and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

    //The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

    //THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
    //FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
    //WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


}



