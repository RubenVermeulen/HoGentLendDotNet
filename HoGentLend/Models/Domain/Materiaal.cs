using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public class Materiaal
    {
        public long Id { get; private set; }

        public string Name
        {
            get; private set; }

        public string Description
        {
            get; private set; }

        public string ArticleCode
        {
            get; private set; }

        public double Price
        {
            get; private set; }

        public int Amount
        {
            get; private set; }

        public int AmountNotAvailable
        {
            get; private set; }

        public bool IsLendable
        {
            get; private set; }


        public string Location
        {
            get; private set; }

        public List<Groep> DoelGroepen
        {
            get; private set;
        }

        public List<Groep> LeerGebieden
        {
            get; private set;
        }

        public Firma Firma
        {
            get; private set;
        }

        public byte[] FotoBytes
        {
            get; private set; }
    }
}