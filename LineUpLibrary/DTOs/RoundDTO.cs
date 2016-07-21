﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.DTOs
{
    public class RoundDTO
    {

        public RoundDTO()
        {
            this.matches = new List<MatchDTO>();
        }

        [Required]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public Nullable<int> round_number { get; set; }
        [Required]
        public Nullable<System.DateTime> start_date { get; set; }
        [Required]
        public Nullable<System.DateTime> end_date { get; set; }
        [Required]
        public int game_type_id { get; set; }
        public Nullable<System.DateTime> lock_date { get; set; }

        public string short_name { get; set; }
        public string game_type_name { get; set; }
        public Nullable<int> round_points_sum { get; set; }
        public decimal? round_winnings { get; set; }
        public double max_pick_count { get; set; }

        public virtual IList<MatchDTO> matches { get; set; }
        public virtual IList<PickDTO> picks { get; set; }

    }
}