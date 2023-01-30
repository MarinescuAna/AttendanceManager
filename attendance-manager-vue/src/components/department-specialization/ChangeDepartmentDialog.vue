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
  DepartmentUpdateModule,
  DepartmentViewModel,
} from "@/modules/department";
import storeHelper from "@/store/store-helper";

export default {
  props: {
    // Department
    department: Object as () => DepartmentViewModel,
  },
  data() {
    return {
      // New name
      name: "",
    };
  },
  computed: {
    isValid(): boolean {
      return this.name == "" || this.name == this.department!.name;
    },
  },
  created(): void {
    // Initialize the field
    if (this.department) {
      this.name = this.department.name;
    }
  },
  methods: {
    /*
     * Save the new name
     */
    async onSaveName(): Promise<void> {
      const result = await storeHelper.departmentStore.updateDepartmentName({
        id: this.department!.id,
        name: this.name,
      } as DepartmentUpdateModule);

      if (result.isSuccess) {
        this.$emit("save", this.name);
      } else {
        window.alert(result.error);
      }
    },
  },
};
</script>