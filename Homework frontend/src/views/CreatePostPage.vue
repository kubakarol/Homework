<template>
    <ion-page>
      <ion-header>
        <ion-toolbar>
          <ion-title>Create Post</ion-title>
        </ion-toolbar>
      </ion-header>
      <ion-content>
        <ion-item>
          <ion-label position="floating">Title</ion-label>
          <ion-input v-model="title"></ion-input>
        </ion-item>
        <ion-item>
          <ion-label position="floating">Content</ion-label>
          <ion-input v-model="content"></ion-input>
        </ion-item>
        <ion-button expand="full" @click="createPost">Create</ion-button>
      </ion-content>
    </ion-page>
  </template>
  
  <script>
  import axios from 'axios';
  import { useStore } from 'vuex';
  
  export default {
    data() {
      return {
        title: '',
        content: ''
      };
    },
    methods: {
      async createPost() {
        try {
          await axios.post('https://localhost:7195/api/Post/create', {
            title: this.title,
            content: this.content
          }, {
            headers: {
              'Authorization': `Bearer ${this.$store.state.token}`
            }
          });
          this.$router.push('/');
        } catch (error) {
          console.error(error);
        }
      }
    },
    setup() {
      const store = useStore();
      return { store };
    }
  };
  </script>
  