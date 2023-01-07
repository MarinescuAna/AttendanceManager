<template>
  <v-row justify="center">
    <v-card width="100%">
      <v-card-title class="ma-3">Change the department name</v-card-title>
      <v-divider></v-divider>
      <v-card-text class="ma-3">
        <v-text-field
          label="New department name"
          v-model="name"
          prepend-icon="mdi-pencil"
          required
        />
      </v-card-text>
      <v-divider></v-divider>
      <v-card-actions class="ma-3">
        <v-btn
          color="blue darken-1"
          text
          @click="onSaveName"
          :disabled="name != '' || name != departmentName"
        >
          Save
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-row>
</template>

<script lang="ts">
export default {
  props: {
    // Department name
    departmentName: String,
  },
  data() {
    return {
      // New name
      name: "",
    };
  },
  created(): void {
    // Initialize the field
    if (this.departmentName) {
      this.name = this.departmentName;
    }
  },
  methods: {
    /*
    * Save the new name
    */
    onSaveName(): void {
      this.$emit("save", this.name);
    },
  },
};
</script>