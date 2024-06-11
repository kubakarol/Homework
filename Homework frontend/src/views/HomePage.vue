<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Home</ion-title>
        <ion-buttons slot="end">
          <ion-button v-if="!isAuthenticated" @click="goToLogin">Login</ion-button>
          <ion-button @click="viewUserPosts" v-if="isAuthenticated" class="custom-button">My Posts</ion-button>
          <ion-button v-if="isAuthenticated" @click="logout" class="custom-button">Logout</ion-button>
        </ion-buttons>
      </ion-toolbar>
    </ion-header>
    <ion-content>
      <div class="search-container">
        <ion-searchbar v-model="searchQuery" @ionInput="searchPosts"></ion-searchbar>
      </div>
      <div class="button-container">
        <ion-button @click="createPost" v-if="isAuthenticated" class="add-post-button">Don't know something? Create a post!</ion-button>
        <ion-button @click="toggleSortOrder" size="small" class="sort-button">
          <i :class="sortOrderIcon"></i>
        </ion-button>
      </div>
      <ion-list>
        <ion-item v-for="(post, index) in sortedPosts" :key="post.id">
          <ion-label>
            <div v-if="editingPost === post">
              <ion-input v-model="editedPostTitle" placeholder="Edit title..."></ion-input>
              <ion-textarea v-model="editedPostContent" placeholder="Edit content..."></ion-textarea>
              <ion-button @click="saveEditedPost">Save</ion-button>
              <ion-button @click="cancelEditPost">Cancel</ion-button>
            </div>
            <div v-else>
              <h2>{{ post.title }}</h2>
              <p>{{ post.content }}</p>
              <p><strong>Posted by: <router-link :to="{ name: 'UserProfile', params: { userId: post.userId } }">{{ post.userName }}</router-link></strong></p>
              <ion-button v-if="isAuthenticated && post.userId === getUser.id" @click="deletePost(post.id, index)" class="delete-button"><i class="bi bi-trash"></i></ion-button>
            </div>
            <h3>Comments</h3>
            <ion-list>
              <ion-item v-for="comment in post.comments" :key="comment.id">
                <ion-label>
                  <div v-if="editingComment === comment">
                    <ion-input v-model="editedCommentContent" placeholder="Edit comment..."></ion-input>
                    <ion-button @click="saveEditedComment">Save</ion-button>
                    <ion-button @click="cancelEditComment">Cancel</ion-button>
                  </div>
                  <div v-else>
                    <strong>{{ comment.userName }}</strong>: {{ comment.content }}
                    <ion-button v-if="isAuthenticated && comment.userId === getUser.id" @click="editComment(comment, post.id, index)"><i class="bi bi-pencil-square"></i></ion-button>
                    <ion-button v-if="isAuthenticated && comment.userId === getUser.id" @click="deleteComment(comment.id, index)"><i class="bi bi-trash"></i></ion-button>
                  </div>
                </ion-label>
              </ion-item>
              <ion-item v-if="isAuthenticated">
                <ion-input v-model="post.newComment" placeholder="Add a comment..."></ion-input>
                <ion-button @click="addComment(post.id, post.newComment, index)">Answer</ion-button>
              </ion-item>
            </ion-list>
          </ion-label>
        </ion-item>
      </ion-list>
    </ion-content>
  </ion-page>
</template>

<script>
import { IonPage, IonHeader, IonToolbar, IonTitle, IonButtons, IonButton, IonContent, IonList, IonItem, IonLabel, IonInput, IonSearchbar, IonTextarea } from '@ionic/vue';
import axios from 'axios';

export default {
  name: 'Home',
  meta: {
    transitionEl: 'ion-content'
  },
  components: { IonPage, IonHeader, IonToolbar, IonTitle, IonButtons, IonButton, IonContent, IonList, IonItem, IonLabel, IonInput, IonSearchbar, IonTextarea },
  data() {
    return {
      sortOrder: 'desc', // domyślnie najnowsze posty na górze
      searchQuery: '',
      editingPost: null,
      editingComment: null,
      editingPostIndex: null,
      editingCommentIndex: null,
      editingCommentPostIndex: null,
      editedPostTitle: '',
      editedPostContent: '',
      editedCommentContent: ''
    };
  },
  computed: {
    isAuthenticated() {
      return !!this.$store.state.token;
    },
    getUser() {
      return this.$store.state.user;
    },
    posts() {
      return this.$store.state.posts;
    },
    sortedPosts() {
      return this.posts.slice().sort((a, b) => {
        if (this.sortOrder === 'asc') {
          return a.id - b.id;
        } else {
          return b.id - a.id;
        }
      });
    },
    sortOrderIcon() {
      return this.sortOrder === 'asc' ? 'fas fa-arrow-up' : 'fas fa-arrow-down';
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
    async searchPosts() {
      if (!this.searchQuery.trim()) {
        this.fetchPosts();
        return;
      }
      try {
        const response = await axios.get('https://localhost:7195/api/Post/search', {
          params: {
            query: this.searchQuery
          }
        });
        const posts = response.data.$values.map(post => ({
          ...post,
          comments: post.comments.$values,
          newComment: ''
        }));
        this.$store.commit('setPosts', posts);
      } catch (error) {
        console.error('Error searching posts:', error);
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
    async deletePost(postId, index) {
      try {
        await axios.delete(`https://localhost:7195/api/Post/${postId}`, {
          headers: {
            'Authorization': `Bearer ${this.$store.state.token}`
          }
        });
        this.posts.splice(index, 1);
      } catch (error) {
        console.error('Error deleting post:', error);
      }
    },
    async deleteComment(commentId, index) {
      try {
        await axios.delete(`https://localhost:7195/api/Post/comment/${commentId}`, {
          headers: {
            'Authorization': `Bearer ${this.$store.state.token}`
          }
        });
        this.posts[index].comments = this.posts[index].comments.filter(comment => comment.id !== commentId);
      } catch (error) {
        console.error('Error deleting comment:', error);
      }
    },
    createPost() {
      this.$router.push('/create-post');
    },
    goToLogin() {
      this.$router.push('/login');
    },
    viewUserPosts() {
      this.$router.push('/user-posts');
    },
    logout() {
      this.$store.commit('clearAuthData');
      this.fetchPosts();
      this.$router.push('/');
    },
    toggleSortOrder() {
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    },
    async editPost(post, index) {
      this.editingPost = post;
      this.editingPostIndex = index;
      this.editedPostTitle = post.title;
      this.editedPostContent = post.content;
    },
    async saveEditedPost() {
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
        this.posts[this.editingPostIndex].title = this.editedPostTitle;
        this.posts[this.editingPostIndex].content = this.editedPostContent;
        this.editingPost = null;
        this.editingPostIndex = null;
      } catch (error) {
        console.error('Error editing post:', error);
      }
    },
    async editComment(comment, index) {
      this.editingComment = comment;
      this.editingCommentIndex = index;
      this.editedCommentContent = comment.content;
    },
    async saveEditedComment() {
      if (!this.editingComment) return;
      try {
        await axios.put(`https://localhost:7195/api/Post/comment/${this.editingComment.id}`, {
          content: this.editedCommentContent
        }, {
          headers: {
            'Authorization': `Bearer ${this.$store.state.token}`
          }
        });
        const postIndex = this.posts.findIndex(post => post.id === this.editingComment.postId);
        const commentIndex = this.posts[postIndex].comments.findIndex(comment => comment.id === this.editingComment.id);
        this.posts[postIndex].comments[commentIndex].content = this.editedCommentContent;
        this.editingComment = null;
        this.editingCommentIndex = null;
      } catch (error) {
        console.error('Error editing comment:', error);
      }
    },
    cancelEditPost() {
      this.editingPost = null;
      this.editingPostIndex = null;
      this.editedPostTitle = '';
      this.editedPostContent = '';
    },
    cancelEditComment() {
      this.editingComment = null;
      this.editingCommentIndex = null;
      this.editedCommentContent = '';
    }
  },
  mounted() {
    this.fetchPosts();
  }
};
</script>

<style scoped>
.button-container {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-wrap: wrap;
  gap: 10px;
}

.add-post-button {
  border-radius: 50px;
  margin: 10px;
}

.custom-button {
  color: black !important;
}

.sort-button {
  background-color: #007bff;
  color: white;
  border-radius: 50%;
  margin: 10px;
}

.search-container {
  display: flex;
  justify-content: center;
  align-items: center;
  margin: 10px;
}
</style>
