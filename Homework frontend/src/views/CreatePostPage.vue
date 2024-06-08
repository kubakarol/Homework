<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Create Post</ion-title>
        <ion-buttons slot="start">
          <ion-back-button></ion-back-button>
        </ion-buttons>
      </ion-toolbar>
    </ion-header>
    <ion-content>
      <ion-list>
        <ion-item>
          <ion-label position="floating">Title</ion-label>
          <ion-input v-model="title"></ion-input>
        </ion-item>
        <ion-item>
          <ion-label position="floating">Content</ion-label>
          <ion-textarea v-model="content"></ion-textarea>
        </ion-item>
      </ion-list>
      <ion-button expand="full" @click="submitPost">Submit</ion-button>
    </ion-content>
  </ion-page>
</template>

<script>
import { IonPage, IonHeader, IonToolbar, IonTitle, IonButtons, IonBackButton, IonContent, IonList, IonItem, IonLabel, IonInput, IonTextarea, IonButton } from '@ionic/vue';
import axios from 'axios';

export default {
  name: 'CreatePost',
  components: {
    IonPage, IonHeader, IonToolbar, IonTitle, IonButtons, IonBackButton, IonContent, IonList, IonItem, IonLabel, IonInput, IonTextarea, IonButton
  },
  data() {
    return {
      title: '',
      content: ''
    };
  },
  computed: {
    userId() {
      const userId = this.$store.state.user ? this.$store.state.user.id : null;
      console.log("Computed userId:", userId); // Log the userId to check if it's correctly retrieved
      return userId;
    }
  },
  methods: {
    async submitPost() {
      if (!this.userId) {
        console.error('User ID not found.');
        return;
      }
      try {
        const postData = {
          title: this.title,
          content: this.content,
          userId: this.userId
        };
        console.log("Submitting post with data:", postData);
        const response = await axios.post('https://localhost:7195/api/Post/create', postData, {
          headers: {
            'Authorization': `Bearer ${this.$store.state.token}`,
            'Content-Type': 'application/json'
          }
        });
        console.log("Post created successfully:", response.data);
        this.$store.dispatch('fetchPosts'); // Fetch posts after successfully adding a new post
        this.$router.push('/');
      } catch (error) {
        console.error('Error creating post:', error.response ? error.response.data : error.message);
      }
    }
  }
};
</script>

<style scoped>
/* Add styling if needed */
</style>
