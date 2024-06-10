<template>
    <ion-page>
      <ion-header>
        <ion-toolbar>
          <ion-title>Your Posts</ion-title>
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
              <ion-button @click="deletePost(post.id, postIndex)"><i class="bi bi-trash"></i></ion-button>
              <h3>Comments</h3>
              <ion-list>
                <ion-item v-for="(comment, commentIndex) in post.comments" :key="comment.id">
                  <ion-label>
                    <strong>{{ comment.userName }}</strong>: {{ comment.content }}
                  </ion-label>
                  <ion-button v-if="comment.userId === userId" @click="deleteComment(comment.id, postIndex, commentIndex)"><i class="bi bi-trash"></i></ion-button>
                </ion-item>
              </ion-list>
            </ion-label>
          </ion-item>
        </ion-list>
      </ion-content>
    </ion-page>
  </template>
  
  <script>
  import { IonPage, IonHeader, IonToolbar, IonTitle, IonButtons, IonBackButton, IonContent, IonList, IonItem, IonLabel, IonButton } from '@ionic/vue';
  import axios from 'axios';
  
  export default {
    name: 'UserPosts',
    components: { IonPage, IonHeader, IonToolbar, IonTitle, IonButtons, IonBackButton, IonContent, IonList, IonItem, IonLabel, IonButton },
    data() {
      return {
        userPosts: []
      };
    },
    computed: {
      userId() {
        return this.$store.state.user ? this.$store.state.user.id : null;
      }
    },
    methods: {
      async fetchUserPosts() {
        if (!this.userId) {
          console.error('User ID not found.');
          return;
        }
        try {
          const response = await axios.get(`https://localhost:7195/api/Post/getByUser/${this.userId}`, {
            headers: {
              'Authorization': `Bearer ${this.$store.state.token}`
            }
          });
          const posts = response.data.$values.map(post => ({
            ...post,
            comments: post.comments.$values,
            newComment: ''
          }));
          this.userPosts = posts;
        } catch (error) {
          console.error('Error fetching user posts:', error);
        }
      },
      async deletePost(postId, postIndex) {
        try {
          await axios.delete(`https://localhost:7195/api/Post/${postId}`, {
            headers: {
              'Authorization': `Bearer ${this.$store.state.token}`
            }
          });
          this.userPosts.splice(postIndex, 1);
        } catch (error) {
          console.error('Error deleting post:', error);
        }
      },
      async deleteComment(commentId, postIndex, commentIndex) {
        try {
          await axios.delete(`https://localhost:7195/api/Post/comment/${commentId}`, {
            headers: {
              'Authorization': `Bearer ${this.$store.state.token}`
            }
          });
          this.userPosts[postIndex].comments.splice(commentIndex, 1);
        } catch (error) {
          console.error('Error deleting comment:', error);
        }
      }
    },
    mounted() {
      this.fetchUserPosts();
    }
  };
  </script>
  