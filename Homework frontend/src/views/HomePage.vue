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
        <ion-item v-for="post in posts" :key="post.id">
          <ion-label>
            <h2>{{ post.title }}</h2>
            <p>{{ post.content }}</p>
            <h3>Comments</h3>
            <ion-list>
              <ion-item v-for="comment in post.comments" :key="comment.id">
                <ion-label>{{ comment.content }}</ion-label>
              </ion-item>
              <ion-item v-if="isAuthenticated">
                <ion-input v-model="newComment" placeholder="Add a comment..."></ion-input>
                <ion-button @click="addComment(post.id)">Submit</ion-button>
              </ion-item>
            </ion-list>
          </ion-label>
        </ion-item>
      </ion-list>
    </ion-content>
  </ion-page>
</template>

<script>
import axios from 'axios';
import { useStore } from 'vuex';

export default {
  data() {
    return {
      posts: [],
      newComment: ''
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
        this.posts = response.data;
      } catch (error) {
        console.error(error);
      }
    },
    async addComment(postId) {
      if (!this.newComment.trim()) return;
      try {
        await axios.post(`https://localhost:7195/api/Post/addComment`, {
          postId,
          content: this.newComment
        }, {
          headers: {
            'Authorization': `Bearer ${this.$store.state.token}`
          }
        });
        this.newComment = '';
        this.fetchPosts();
      } catch (error) {
        console.error(error);
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
