<template>
    <ion-page>
      <ion-header>
        <ion-toolbar>
          <ion-title>{{ userName }}'s Posts</ion-title>
          <ion-buttons slot="start">
            <ion-back-button></ion-back-button>
          </ion-buttons>
        </ion-toolbar>
      </ion-header>
      <ion-content>
        <ion-list>
          <ion-item v-for="(post, postIndex) in userPosts" :key="post.id">
            <ion-label>
              <h2>{{ post.title }}</h2>
              <p>{{ post.content }}</p>
              <h3>Comments</h3>
              <ion-list>
                <ion-item v-for="(comment, commentIndex) in post.comments" :key="comment.id">
                  <ion-label>
                    <strong>{{ comment.userName }}</strong>: {{ comment.content }}
                  </ion-label>
                </ion-item>
              </ion-list>
            </ion-label>
          </ion-item>
        </ion-list>
      </ion-content>
    </ion-page>
  </template>
  
  <script>
  import { IonPage, IonHeader, IonToolbar, IonTitle, IonButtons, IonBackButton, IonContent, IonList, IonItem, IonLabel } from '@ionic/vue';
  import axios from 'axios';
  
  export default {
    name: 'UserProfile',
    components: { IonPage, IonHeader, IonToolbar, IonTitle, IonButtons, IonBackButton, IonContent, IonList, IonItem, IonLabel },
    data() {
      return {
        userPosts: [],
        userName: ''
      };
    },
    methods: {
      async fetchUserPosts() {
        const userId = this.$route.params.userId;
        if (!userId) {
          console.error('User ID not found.');
          return;
        }
        try {
          const response = await axios.get(`https://localhost:7195/api/Post/getByUser/${userId}/public`);
          const posts = response.data.$values.map(post => ({
            ...post,
            comments: post.comments.$values,
            newComment: ''
          }));
          this.userPosts = posts;
          this.userName = this.userPosts[0]?.userName || 'User'; // Set the userName from the first post's userName
        } catch (error) {
          console.error('Error fetching user posts:', error);
        }
      }
    },
    mounted() {
      this.fetchUserPosts();
    }
  };
  </script>
  
  <style scoped>
  /* Add any custom styles here if needed */
  </style>
  