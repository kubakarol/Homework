<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Home</ion-title>
        <ion-buttons slot="end">
          <ion-button v-if="!isAuthenticated" @click="goToLogin">Login</ion-button>
          <ion-button v-if="isAuthenticated" @click="logout">Logout</ion-button>
        </ion-buttons>
      </ion-toolbar>
    </ion-header>
    <ion-content>
      <ion-button expand="full" @click="createPost" v-if="isAuthenticated">Create Post</ion-button>
      <ion-list>
        <ion-item v-for="(post, index) in posts" :key="post.id">
          <ion-label>
            <h2>{{ post.title }}</h2>
            <p>{{ post.content }}</p>
            <p><strong>Posted by: {{ post.userName }}</strong></p>
            <h3>Comments</h3>
            <ion-list>
              <ion-item v-for="comment in post.comments" :key="comment.id">
                <ion-label>
                  <strong>{{ comment.userName }}</strong>: {{ comment.content }}
                </ion-label>
              </ion-item>
              <ion-item v-if="isAuthenticated">
                <ion-input v-model="post.newComment" placeholder="Add a comment..."></ion-input>
                <ion-button @click="addComment(post.id, post.newComment, index)">Submit</ion-button>
              </ion-item>
            </ion-list>
          </ion-label>
        </ion-item>
      </ion-list>
    </ion-content>
  </ion-page>
</template>

<script>
import { IonPage, IonHeader, IonToolbar, IonTitle, IonButtons, IonButton, IonContent, IonList, IonItem, IonLabel, IonInput } from '@ionic/vue';
import axios from 'axios';

export default {
  name: 'Home',
  components: {
    IonPage, IonHeader, IonToolbar, IonTitle, IonButtons, IonButton, IonContent, IonList, IonItem, IonLabel, IonInput
  },
  computed: {
    isAuthenticated() {
      return !!this.$store.state.token;
    },
    posts() {
      return this.$store.state.posts;
    }
  },
  methods: {
    async fetchPosts() {
      try {
        const response = await axios.get('https://localhost:7195/api/Post/getAll');
        console.log(response.data);
        const posts = response.data.$values.map(post => ({
          ...post,
          comments: post.comments.$values,
          newComment: ''
        }));
        this.$store.commit('setPosts', posts);
      } catch (error) {
        console.error('Error fetching posts:', error);
      }
    },
    async addComment(postId, newComment, index) {
      if (!newComment.trim()) return;
      try {
        await axios.post('https://localhost:7195/api/Post/addComment', {
          postId,
          content: newComment
        }, {
          headers: {
            'Authorization': `Bearer ${this.$store.state.token}`
          }
        });
        this.posts[index].newComment = '';
        this.fetchPosts();
      } catch (error) {
        console.error('Error adding comment:', error);
      }
    },
    createPost() {
      this.$router.push('/create-post');
    },
    goToLogin() {
      this.$router.push('/login');
    },
    logout() {
      this.$store.commit('clearAuthData');
      this.fetchPosts(); // Odśwież listę postów po wylogowaniu
      this.$router.push('/');
    }
  },
  mounted() {
    this.fetchPosts();
  }
};
</script>
