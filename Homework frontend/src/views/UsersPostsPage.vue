<template>
    <ion-page>
      <ion-header>
        <ion-toolbar>
          <ion-title>Your Posts</ion-title>
        </ion-toolbar>
      </ion-header>
      <ion-content>
        <ion-list>
          <ion-item v-for="(post, index) in userPosts" :key="post.id">
            <ion-label>
              <h2>{{ post.title }}</h2>
              <p>{{ post.content }}</p>
              <h3>Comments</h3>
              <ion-list>
                <ion-item v-for="comment in post.comments" :key="comment.id">
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
  import { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonList, IonItem, IonLabel } from '@ionic/vue';
  import axios from 'axios';
  
  export default {
    name: 'UserPosts',
    components: { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonList, IonItem, IonLabel },
    computed: {
      userId() {
        return this.$store.state.user ? this.$store.state.user.id : null;
      },
      userPosts() {
        return this.$store.state.userPosts;
      }
    },
    methods: {
      async fetchUserPosts() {
        if (!this.userId) {
          console.error('User ID not found.');
          return;
        }
        try {
          await this.$store.dispatch('fetchUserPosts');
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
  