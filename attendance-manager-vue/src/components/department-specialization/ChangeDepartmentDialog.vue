<template>
  <v-card>
    <v-card-title class="ma-3">Change the department name     <v-spacer></v-spacer>
      <v-btn @click="$emit('close')" icon> <v-icon>mdi-close</v-icon> </v-btn></v-card-title>
    <v-card-text class="pa-4">
      <v-text-field
        label="New department name"
        v-model="name"
        prepend-icon="mdi-pencil"
        required
      />
    </v-card-text>
    <v-card-actions class="ma-3">
      <v-row justify="center" class="pa-8">
      <v-btn color="dark_button white--text" @click="onSaveName" :disabled="isValid">
        Save
      </v-btn>
      </v-row>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
import { UpdateDepartmentParameters } from "@/modules/commands-parameters";
import { DepartmentViewModule } from "@/modules/view-modules";
import storeHelper from "@/store/store-helper";

export default {
  name:"ChangeDepartmentDialog",
  props: {
      /** Department object */
      department: {
          type: Object as () => DepartmentViewModule,
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
      const result = await storeHelper.departmentStore.updateDepartment({
        departmentId: this.department.id,
        name: this.name,
      } as UpdateDepartmentParameters);

      if (result) {
        this.$emit("save", this.name);
      }
    },
  },
};
</script>