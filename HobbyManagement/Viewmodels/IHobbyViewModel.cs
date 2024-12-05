﻿using HobbyManagement.Business;

namespace HobbyManagement.Viewmodels;

public interface IHobbyViewModel
{
    string Description { get; }
    IEditHobbyViewModel? EditHobbyData { get; }
    int Id { get; }
    bool IsEditing { get; set; }
    string Name { get; }
    string HobbyAsCSV();
    string HobbyHeaderNamesAsCsv();
    bool IsEmpty();
    public void SetAsUpdated();
    void SetWrappedHobby(IHobby hobby);
    void StartEdit();
}