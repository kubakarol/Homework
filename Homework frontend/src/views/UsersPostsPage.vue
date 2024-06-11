<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Your Posts</ion-title>
        <ion-buttons slot="start">
          <ion-back-button defaultHref="/"></ion-back-button>
        </ion-buttons>
      </ion-toolbar>
    </ion-header>
    <ion-content>
      <ion-list>
        <ion-item v-for="(post, postIndex) in userPosts" :key="post.id">
          <ion-label>
            <div v-if="editingPost === post">
              <ion-input v-model="editedPostTitle" placeholder="Edit title..."></ion-input>
              <ion-textarea v-model="editedPostContent" placeholder="Edit content..."></ion-textarea>
              <ion-button @click="saveEditedPost(postIndex)">Save</ion-button>
              <ion-button @click="cancelEditPost">Cancel</ion-button>
            </div>
            <div v-else>
              <h2>{{ post.title }}</h2>
              <p>{{ post.content }}</p>
              <p><strong>Posted by: <router-link :to="{ name: 'UserProfile', params: { userId: post.userId } }">{{ post.userName }}</router-link></strong></p>
              <ion-button @click="deletePost(post.id, postIndex)"><i class="bi bi-trash"></i></ion-button>
            </div>
            <h3>Comments</h3>
            <ion-list>
              <ion-item v-for="(comment, commentIndex) in post.comments" :key="comment.id">
                <ion-label>
                  <div v-if="editingComment === comment">
                    <ion-input v-model="editedCommentContent" placeholder="Edit comment..."></ion-input>
                    <ion-button @click="saveEditedComment(postIndex, commentIndex)">Save</ion-button>
                    <ion-button @click="cancelEditComment">Cancel</ion-button>
                  </div>
                  <div v-else>
                    <strong>{{ comment.userName }}</strong>: {{ comment.content }}
                    <ion-button v-if="comment.userId === userId" @click="deleteComment(comment.id, postIndex, commentIndex)"><i class="bi bi-trash"></i></ion-button>
                  </div>
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
import { IonPage, IonHeader, IonToolbar, IonTitle, IonButtons, IonBackButton, IonContent, IonList, IonItem, IonLabel, IonButton, IonInput, IonTextarea } from '@ionic/vue';
import axios from 'axios';

export default {
  name: 'UserPosts',
  meta: {
    transitionEl: 'ion-content'
  },
  components: { IonPage, IonHeader, IonToolbar, IonTitle, IonButtons, IonBackButton, IonContent, IonList, IonItem, IonLabel, IonButton, IonInput, IonTextarea },
  data() {
    return {
      userPosts: [],
      editingPost: null,
      editedPostTitle: '',
      editedPostContent: '',
      editingComment: null,
      editedCommentContent: ''
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
    },
    editPost(post, postIndex) {
      this.editingPost = post;
      this.editedPostTitle = post.title;
      this.editedPostContent = post.content;
    },
    cancelEditPost() {
      this.editingPost = null;
      this.editedPostTitle = '';
      this.editedPostContent = '';
    },
    async saveEditedPost(postIndex) {
      if (!this.editingPost) return;
      try {
        await axios.put(`https://localhost:7195/api/Post/${this.editingPost.id}`, {
          title: this.editedPostTitle,
          content: this.editedPostContent
        }, {
          headers: {
            'Authorization': `Bearer ${this.$store.state.token}`
          }
        });
        this.userPosts[postIndex].title = this.editedPostTitle;
        this.userPosts[postIndex].content = this.editedPostContent;
        this.editingPost = null;
        this.editedPostTitle = '';
        this.editedPostContent = '';
      } catch (error) {
        console.error('Error editing post:', error);
      }
    },
    editComment(comment, postIndex, commentIndex) {
      this.editingComment = comment;
      this.editedCommentContent = comment.content;
      this.editingCommentPostIndex = postIndex;
      this.editingCommentIndex = commentIndex;
    },
    cancelEditComment() {
      this.editingComment = null;
      this.editedCommentContent = '';
      this.editingCommentPostIndex = null;
      this.editingCommentIndex = null;
    },
    async saveEditedComment(postIndex, commentIndex) {
      if (!this.editingComment) return;
      try {
        await axios.put(`https://localhost:7195/api/Post/comment/${this.editingComment.id}`, {
          content: this.editedCommentContent
        }, {
          headers: {
            'Authorization': `Bearer ${this.$store.state.token}`
          }
        });
        this.userPosts[postIndex].comments[commentIndex].content = this.editedCommentContent;
        this.editingComment = null;
        this.editedCommentContent = '';
        this.editingCommentPostIndex = null;
        this.editingCommentIndex = null;
      } catch (error) {
        console.error('Error editing comment:', error);
      }
    }
  },
  mounted() {
    this.fetchUserPosts();
  }
};
</script>

<style scoped>
.edit-button {
  margin-left: 10px;
}

.delete-button {
  margin-left: 10px;
}
</style>