<template>
  <v-card>
    <v-card-title class="pa-3">Change the department name</v-card-title>
    <v-divider></v-divider>
    <v-card-text class="pa-4">
      <v-text-field
        label="New department name"
        v-model="name"
        prepend-icon="mdi-pencil"
        required
      />
    </v-card-text>
    <v-divider></v-divider>
    <v-card-actions class="ma-3">
      <v-btn color="blue darken-1" text @click="onSaveName" :disabled="isValid">
        Save
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
import {
  DepartmentModule,
} from "@/modules/department";
import storeHelper from "@/store/store-helper";

export default {
  name:"ChangeDepartmentDialog",
  props: {
      /** Department object */
      department: {
          type: Object as () => DepartmentModule,
          required: true
      }
  },
  data: function() {
    return {
      /** The new name of the department */ 
      name: "",
    };
  },
  computed: {
    isValid: function(): boolean {
      return this.name == "" || this.name == this.department.name;
    },
  },
  /** Initialize the department name */
  created: function(): void {
    if (this.department) {
      this.name = this.department.name;
    }
  },
  methods: {
    /** Save the new name and emit a save event to the parent */
    onSaveName: async function(): Promise<void> {
      const result = await storeHelper.departmentStore.updateDepartmentName({
        id: this.department.id,
        name: this.name,
      } as DepartmentModule);

      if (result) {
        this.$emit("save", this.name);
      }
    },
  },
};
</script>