using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    [Header("Pools")]
    public ObjectPool<Bullet> bulletPool;                           // 총알 풀
    public ObjectPool<CartridgeCase> cartridgeCasePool;             // 탄피 풀
    public ObjectPool<BulletHole> bulletHolePool;                   // 탄흔 풀
    public ObjectPool<BloodParticleCtrl> bloodParticlePool;         // 피 파티클 풀
    public ObjectPool<ArtilleryAmmo> artilleryAmmoPool;             // 야포 포탄 풀
    public ObjectPool<KoreanMaleCtrl> koreanMaleCtrlPool;           // 피난민 풀

    [Header("Prefabs")]
    [SerializeField]
    private GameObject bulletPrefab;                                // 총알 프리팹
    [SerializeField]
    private GameObject cartridgeCasePrefab;                         // 탄피 프리팹
    [SerializeField]
    private GameObject[] bulletHoleConcretePrefabs;                 // 콘크리트 탄흔 프리팹
    [SerializeField]
    private GameObject bloodParticlePrefabs;                        // 피 파티클 프리팹
    [SerializeField]
    private GameObject artilleryAmmoPrefabs;                        // 야포 포탄 파티클 프리팹
    [SerializeField]
    private GameObject[] koreanMaleCtrlPrefabs;                     // 피난민 프리팹


    private void Awake()
    {
        bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: 100);
        cartridgeCasePool = new ObjectPool<CartridgeCase>(CreateCartridgeCase, OnGetCartridgeCase, OnReleaseCartridgeCase, OnDestroyCartridgeCase, maxSize: 100);
        bulletHolePool = new ObjectPool<BulletHole>(CreateBulletHole, OnGetBulletHole, OnReleaseBulletHole, OnDestroyBulletHole, maxSize: 100);
        bloodParticlePool = new ObjectPool<BloodParticleCtrl>(CreateBloodParticle, OnGetBloodParticle, OnReleaseBloodParticle, OnDestroyBloodParticle, maxSize: 100);
        artilleryAmmoPool = new ObjectPool<ArtilleryAmmo>(CreateArtilleryAmmo, OnGetArtilleryAmmo, OnReleaseArtilleryAmmo, OnDestroyArtilleryAmmo, maxSize: 100);
        koreanMaleCtrlPool = new ObjectPool<KoreanMaleCtrl>(CreateKoreanMaleCtrl, OnGetKoreanMaleCtrl, OnReleaseKoreanMaleCtrl, OnDestroyKoreanMaleCtrl, maxSize: 100);
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /*
     * Bullet Pool
     * 
     * CreateBullet         총알 풀 생성
     * OnGetBullet          총알 풀에서 가져오기
     * OnReleaseBullet      총알 풀에 반납하기
     * OnDestroyBullet      총알 풀에서 삭제하기
     */

    // 총알 풀 생성
    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
        bullet.SetManagedPool(bulletPool);

        return bullet;
    }

    // 총알 풀에서 가져오기
    private void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    // 총알 풀에 반납하기
    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    // 총알 풀에서 삭제하기
    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /*
     * CartridgeCase Pool
     * 
     * CreateCartridgeCase         탄피 풀 생성
     * OnGetCartridgeCase          탄피 풀에서 가져오기
     * OnReleaseCartridgeCase      탄피 풀에 반납하기
     * OnDestroyCartridgeCase      탄피 풀에서 삭제하기
     */

    // 탄피 풀 생성
    private CartridgeCase CreateCartridgeCase()
    {
        CartridgeCase cartridgeCase = Instantiate(cartridgeCasePrefab).GetComponent<CartridgeCase>();
        cartridgeCase.SetManagedPool(cartridgeCasePool);

        return cartridgeCase;
    }

    // 탄피 풀에서 가져오기
    private void OnGetCartridgeCase(CartridgeCase cartridgeCase)
    {
        cartridgeCase.gameObject.SetActive(true);
    }

    // 탄피 풀에 반납하기
    private void OnReleaseCartridgeCase(CartridgeCase cartridgeCase)
    {
        cartridgeCase.gameObject.SetActive(false);
    }

    // 탄피 풀에서 삭제하기
    private void OnDestroyCartridgeCase(CartridgeCase cartridgeCase)
    {
        Destroy(cartridgeCase.gameObject);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /*
     * BulletHole Pool
     * 
     * CreateBulletHole         탄흔 풀 생성
     * OnGetBulletHole          탄흔 풀에서 가져오기
     * OnReleaseBulletHole      탄흔 풀에 반납하기
     * OnDestroyBulletHole      탄흔 풀에서 삭제하기
     */

    // 탄흔 풀 생성
    private BulletHole CreateBulletHole()
    {
        int randomIndex = Random.Range(0, bulletHoleConcretePrefabs.Length - 1);
        BulletHole bulletHole = Instantiate(bulletHoleConcretePrefabs[randomIndex]).GetComponent<BulletHole>();
        bulletHole.SetManagedPool(bulletHolePool);

        return bulletHole;
    }

    // 탄흔 풀에서 가져오기
    private void OnGetBulletHole(BulletHole bulletHole)
    {
        bulletHole.gameObject.SetActive(true);
    }

    // 탄흔 풀에 반납하기
    private void OnReleaseBulletHole(BulletHole bulletHole)
    {
        bulletHole.gameObject.SetActive(false);
    }

    // 탄흔 풀에서 삭제하기
    private void OnDestroyBulletHole(BulletHole bulletHole)
    {
        Destroy(bulletHole.gameObject);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /*
     * BloodParticle Pool
     * 
     * CreateBulletHole         피 파티클 풀 생성
     * OnGetBulletHole          피 파티클 풀에서 가져오기
     * OnReleaseBulletHole      피 파티클 풀에 반납하기
     * OnDestroyBulletHole      피 파티클 풀에서 삭제하기
     */

    // 피 파티클 풀 생성
    private BloodParticleCtrl CreateBloodParticle()
    {
        BloodParticleCtrl bloodParticleCtrl = Instantiate(bloodParticlePrefabs).GetComponent<BloodParticleCtrl>();
        bloodParticleCtrl.SetManagedPool(bloodParticlePool);

        return bloodParticleCtrl;
    }

    // 피 파티클 풀에서 가져오기
    private void OnGetBloodParticle(BloodParticleCtrl bloodParticleCtrl)
    {
        bloodParticleCtrl.gameObject.SetActive(true);
    }

    // 피 파티클 풀에 반납하기
    private void OnReleaseBloodParticle(BloodParticleCtrl bloodParticleCtrl)
    {
        bloodParticleCtrl.gameObject.SetActive(false);
    }

    // 피 파티클 풀에서 삭제하기
    private void OnDestroyBloodParticle(BloodParticleCtrl bloodParticleCtrl)
    {
        Destroy(bloodParticleCtrl.gameObject);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /*
     * ArtilleryAmmo Pool
     * 
     * CreateArtilleryAmmo         야포 포탄 파티클 풀 생성
     * OnGetArtilleryAmmo          야포 포탄 파티클 풀에서 가져오기
     * OnReleaseArtilleryAmmo      야포 포탄 파티클 풀에 반납하기
     * OnDestroyArtilleryAmmo      야포 포탄 파티클 풀에서 삭제하기
     */

    // 야포 포탄 파티클 풀 생성
    private ArtilleryAmmo CreateArtilleryAmmo()
    {
        ArtilleryAmmo artilleryAmmo = Instantiate(artilleryAmmoPrefabs).GetComponent<ArtilleryAmmo>();
        artilleryAmmo.SetManagedPool(artilleryAmmoPool);

        return artilleryAmmo;
    }

    // 야포 포탄 파티클 풀에서 가져오기
    private void OnGetArtilleryAmmo(ArtilleryAmmo artilleryAmmo)
    {
        artilleryAmmo.gameObject.SetActive(true);
    }

    // 야포 포탄 파티클 풀에 반납하기
    private void OnReleaseArtilleryAmmo(ArtilleryAmmo artilleryAmmo)
    {
        artilleryAmmo.gameObject.SetActive(false);
    }

    // 야포 포탄 파티클 풀에서 삭제하기
    private void OnDestroyArtilleryAmmo(ArtilleryAmmo artilleryAmmo)
    {
        Destroy(artilleryAmmo.gameObject);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /*
     * ArtilleryAmmo Pool
     * 
     * CreateArtilleryAmmo         야포 포탄 파티클 풀 생성
     * OnGetArtilleryAmmo          야포 포탄 파티클 풀에서 가져오기
     * OnReleaseArtilleryAmmo      야포 포탄 파티클 풀에 반납하기
     * OnDestroyArtilleryAmmo      야포 포탄 파티클 풀에서 삭제하기
     */

    // 야포 포탄 파티클 풀 생성
    private KoreanMaleCtrl CreateKoreanMaleCtrl()
    {
        int randomIndex = Random.Range(0, koreanMaleCtrlPrefabs.Length);

        KoreanMaleCtrl koreanMaleCtrl = Instantiate(koreanMaleCtrlPrefabs[randomIndex]).GetComponent<KoreanMaleCtrl>();
        koreanMaleCtrl.SetManagedPool(koreanMaleCtrlPool);

        return koreanMaleCtrl;
    }

    // 야포 포탄 파티클 풀에서 가져오기
    private void OnGetKoreanMaleCtrl(KoreanMaleCtrl koreanMaleCtrl)
    {
        koreanMaleCtrl.gameObject.SetActive(true);
    }

    // 야포 포탄 파티클 풀에 반납하기
    private void OnReleaseKoreanMaleCtrl(KoreanMaleCtrl koreanMaleCtrl)
    {
        koreanMaleCtrl.gameObject.SetActive(false);
    }

    // 야포 포탄 파티클 풀에서 삭제하기
    private void OnDestroyKoreanMaleCtrl(KoreanMaleCtrl koreanMaleCtrl)
    {
        Destroy(koreanMaleCtrl.gameObject);
    }
}
