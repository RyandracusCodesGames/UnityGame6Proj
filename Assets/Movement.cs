using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float xInput, yInput;
    float velocity = 5.0f;
    float tub1Velocity = 0.008f * 3;
    float jellyVelocity = 0.048f;
    float jumpForce = 25.0f;
    bool isWizardState = false, canPress = true;
    bool isJumping = false;
    bool tub1Forwards = true, tub2Up = true, jelly1Up = true, jelly2Up = false, jelly3Up = true, jelly4Up = false;
    Rigidbody2D rb;
    Animator anim;
    public SpriteRenderer sprite;
    GameObject player, tub1, tub2, tub3, tub4, tub5, tub6, jelly1, jelly2, jelly3, jelly4, leftFloor, rightFloor, floor1, floor2, floor3, floor4;
    GameObject boulder1, boulder2;
    GameObject conch1, conch2, conch3, conch4, conch5, conch6, conch7, conch8;
    BoxCollider2D playerBox, tub1Box, leftFloorBox, rightFloorBox;
    AudioSource audio, collectSound, bubblesSound;
    int lives = 5;
    int num_of_conch_collected = 0;
    int max_num_of_conch = 8;
    bool conchCollected = false;
    bool gameOver = false;
    public float maxDistance = 4f;
    GameObject livesText;
    TextMesh livesMesh;
    Rigidbody2D livesRb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        player = GameObject.Find("Player");
        audio = player.GetComponent<AudioSource>();
        leftFloor = GameObject.Find("LeftFloor");
        floor1 = GameObject.Find("Floor1");
        floor2 = GameObject.Find("Floor2");
        floor3 = GameObject.Find("Floor3");
        floor4 = GameObject.Find("Floor4");
        rightFloor = GameObject.Find("RightFloor");
        tub1 = GameObject.Find("Tub1");
        tub2 = GameObject.Find("Tub2");
        tub3 = GameObject.Find("Tub3");
        tub4 = GameObject.Find("Tub4");
        tub5 = GameObject.Find("Tub5");
        tub6 = GameObject.Find("Tub6");
        collectSound = tub1.GetComponent<AudioSource>();
        jelly1 = GameObject.Find("jellyfish_1");
        bubblesSound = jelly1.GetComponent<AudioSource>();
        jelly2 = GameObject.Find("jellyfish_2");
        jelly3 = GameObject.Find("jellyfish_3");
        jelly4 = GameObject.Find("jellyfish_4");
        boulder1 = GameObject.Find("boulder_1");
        boulder2 = GameObject.Find("boulder_2");
        conch1 = GameObject.Find("conch_1");
        conch2 = GameObject.Find("conch_2");
        conch3 = GameObject.Find("conch_3");
        conch4 = GameObject.Find("conch_4");
        conch5 = GameObject.Find("conch_5");
        conch6 = GameObject.Find("conch_6");
        conch7 = GameObject.Find("conch_7");
        conch8 = GameObject.Find("conch_8");

        playerBox = player.GetComponent<BoxCollider2D>();
        tub1Box = tub1.GetComponent<BoxCollider2D>();
        leftFloorBox = leftFloor.GetComponent<BoxCollider2D>();
        rightFloorBox = rightFloor.GetComponent<BoxCollider2D>();

        livesText = new GameObject("LivesText");
        livesMesh = livesText.AddComponent<TextMesh>();
        livesRb = livesText.AddComponent<Rigidbody2D>();
        livesRb.bodyType = RigidbodyType2D.Kinematic;
        livesMesh.text = "LIVES = " + lives.ToString();
        livesMesh.fontSize = 24;
        livesMesh.color = Color.white;

        livesText.transform.localScale = Vector3.one * 0.2f;
        
        livesText.transform.position = new Vector3(-4.87f, 2, 0);
        
        MeshRenderer meshRenderer = livesText.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = "UI";
        meshRenderer.sortingOrder = 10;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == tub1 || collision.gameObject == tub2 || collision.gameObject == tub3 || 
        collision.gameObject == floor1 || collision.gameObject == floor2 || collision.gameObject == floor3 || collision.gameObject == floor4 ||
        collision.gameObject == tub4 || collision.gameObject == tub5 || collision.gameObject == tub6)
        {
            isJumping = false;
        }

        if(collision.gameObject.CompareTag("spikes") || collision.gameObject == jelly1 || collision.gameObject == jelly2 ||
        collision.gameObject == jelly3 || collision.gameObject == jelly4 || collision.gameObject == boulder1 || collision.gameObject == boulder2)
        {
            lives--;
            player.transform.position = new Vector3(0,0,0);
        }

        conchCollected = false;

        if(collision.gameObject == conch1){
            Destroy(conch1);
            num_of_conch_collected++;
            conchCollected = true;
        }

        if(collision.gameObject == conch2){
            Destroy(conch2);
            num_of_conch_collected++;
            conchCollected = true;
        }

        if(collision.gameObject == conch3){
            Destroy(conch3);
            num_of_conch_collected++;
            conchCollected = true;
        }

        if(collision.gameObject == conch4){
            Destroy(conch4);
            num_of_conch_collected++;
            conchCollected = true;
        }

        if(collision.gameObject == conch5){
            Destroy(conch5);
            num_of_conch_collected++;
            conchCollected = true;
        }

        if(collision.gameObject == conch6){
            Destroy(conch6);
            num_of_conch_collected++;
            conchCollected = true;
        }

        if(collision.gameObject == conch7){
            Destroy(conch7);
            num_of_conch_collected++;
            conchCollected = true;
        }

        if(collision.gameObject == conch8){
            Destroy(conch8);
            num_of_conch_collected++;
            conchCollected = true;
        }

        if(conchCollected){
            collectSound.Play();
        }
    }

    IEnumerator ButtonCooldownRoutine()
    {
        canPress = false;
        yield return new WaitForSeconds(0.5f);
        canPress = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        tub1.transform.rotation = Quaternion.Euler(0, 0, 0);
        jelly1.transform.rotation = Quaternion.Euler(0, 0, 0);
        jelly2.transform.rotation = Quaternion.Euler(0, 0, 0);
        jelly3.transform.rotation = Quaternion.Euler(0, 0, 0);
        jelly4.transform.rotation = Quaternion.Euler(0, 0, 0);
        boulder1.transform.rotation = Quaternion.Euler(0, 0, 0);
        boulder2.transform.rotation = Quaternion.Euler(0, 0, 0);

        if(num_of_conch_collected == max_num_of_conch){
            gameOver = true;
        }

        livesMesh.text = "LIVES = " + lives.ToString() + "\n" + "Conches Collected = " + num_of_conch_collected.ToString();

        float distance = Vector3.Distance(player.transform.position, jelly1.transform.position);
        float volume = Mathf.Clamp01(1 - (distance / maxDistance)); // Normalize volume based on distance
        bubblesSound.volume = volume;

        if(player.transform.position.y < -100.0f){
            lives--;
            player.transform.position = new Vector3(0,0,0);
        }

        Vector3 position = tub1.transform.position;

        if(position.x < 11.33f){
            tub1Forwards = false;
        }

        if(position.x > 17.79f){
            tub1Forwards = true;
        }

        if(tub1Forwards){
            position.x -= tub1Velocity;
        }
        else{
            position.x += tub1Velocity;
        }

        tub1.transform.position = new Vector3(position.x, position.y, position.z);

        position = jelly1.transform.position;

        if(position.y > 2.10f){
            jelly1Up = false;
        }

        if(position.y < 0){
            jelly1Up = true;
        }

        if(jelly1Up){
            position.y += jellyVelocity;
        }else{
            position.y -= jellyVelocity;
        }

        jelly1.transform.position = new Vector3(position.x, position.y, position.z);

        position = jelly2.transform.position;

        if(position.y > 2.10f){
            jelly2Up = false;
        }

        if(position.y < 0){
            jelly2Up = true;
        }

        if(jelly2Up){
            position.y += jellyVelocity;
        }else{
            position.y -= jellyVelocity;
        }

        jelly2.transform.position = new Vector3(position.x, position.y, position.z);

        position = jelly3.transform.position;

        if(position.y > 2.10f){
            jelly3Up = false;
        }

        if(position.y < 0){
            jelly3Up = true;
        }

        if(jelly3Up){
            position.y += jellyVelocity;
        }else{
            position.y -= jellyVelocity;
        }

        jelly3.transform.position = new Vector3(position.x, position.y, position.z);

        position = jelly4.transform.position;

        if(position.y > 2.10f){
            jelly4Up = false;
        }

        if(position.y < 0){
            jelly4Up = true;
        }

        if(jelly4Up){
            position.y += jellyVelocity;
        }else{
            position.y -= jellyVelocity;
        }

        jelly4.transform.position = new Vector3(position.x, position.y, position.z);

        float tub3x = tub3.transform.position.x;
        position = tub2.transform.position;

        if(position.y > 4.49){
            tub2Up = false;
        }

        if(position.y < -0.61f){
            tub2Up = true;
        }

        if(tub2Up){
            position.y += jellyVelocity;
        }else{
            position.y -= jellyVelocity;
        }

        tub2.transform.position = new Vector3(position.x, position.y, position.z);
        tub3.transform.position = new Vector3(tub3x, position.y, position.z);

        Vector3 position1 = boulder1.transform.position;
        Vector3 position2 = boulder2.transform.position;

        if(position1.y < -1.49f){
            position1.y = 6.08f;
            position2.y = 6.08f;
        }

        position1.y -= jellyVelocity;
        position2.y -= jellyVelocity;

        boulder1.transform.position = new Vector3(position1.x, position1.y, position1.z);
        boulder2.transform.position = new Vector3(position2.x, position2.y, position2.z);

        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(velocity * xInput, rb.velocity.y);
        livesRb.velocity = new Vector2(velocity * xInput, livesRb.velocity.y);
        anim.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
        anim.SetFloat("yVelocity", rb.velocity.y);

        if(xInput < 0){
            sprite.flipX = true;
        }
        else{
            sprite.flipX = false;
        }

        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && !isJumping && player.transform.position.y > -1.0f){
            isJumping = true;
            rb.velocity = Vector2.up * jumpForce;
        }

        if(player.transform.position.y <= 0){
            isJumping = false;
        }

        if(Input.GetKey(KeyCode.E) && canPress){
            isWizardState = !isWizardState;

            if(isWizardState){
                audio.Play();
            }
            else{
                audio.Stop();
            }

            StartCoroutine(ButtonCooldownRoutine());
        }

        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isWizardState", isWizardState);

        distance = Vector3.Distance(player.transform.position, livesText.transform.position);
    }
}
