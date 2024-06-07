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
            <h3>Comments</h3>
            <ion-list>
              <ion-item v-for="comment in post.comments" :key="comment.id">
                <ion-label>{{ comment.content }}</ion-label>
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
import { useStore } from 'vuex';

export default {
  name: 'Home',
  components: {
    IonPage, IonHeader, IonToolbar, IonTitle, IonButtons, IonButton, IonContent, IonList, IonItem, IonLabel, IonInput
  },
  data() {
    return {
      posts: []
    };
  },
  computed: {
    isAuthenticated() {
      return !!this.$store.state.token;
    }
  },
  methods: {
    async fetchPosts() {
      try {
        const response = await axios.get('https://localhost:7195/api/Post/getAll');
        // Add a newComment property to each post for local state
        this.posts = response.data.map(post => ({ ...post, newComment: '' }));
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
        this.posts[index].newComment = ''; // Clear the comment input
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
      this.$store.commit('logout');
      this.$router.push('/login');
    }
  },
  mounted() {
    this.fetchPosts();
  }
};
</script>
